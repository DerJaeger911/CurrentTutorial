using Godot;
using Metroidvania.game;
using Metroidvania.game.interfaces;
using Metroidvania.game.utility;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D, IDamageable
{
	[ExportGroup("Stats")]
	[Export]
	private int health = 5;

	[ExportGroup("Movement")]
	[Export]
	private float speed = 120f;
	[Export]
	private float acceleration = 600f;
	[Export]
	private float friction = 600f;
	[Export]
	private float dashSpeed = 600f;
	[Export]
	private bool isDucking = false;

	[ExportGroup("Jumping")]
	[Export]
	private float jumpHeight = 100f;
	[Export]
	private float jumpTimeToPeak = 0.5f;
	[Export]
	private float jumpTimeToDescent = 0.4f;
	[Export]
	private bool allowedToCoyoteJump = true;
	private bool wasOnFloorLastFrame;

	[ExportGroup("UI")]
	[Export]
	private int crosshairDistance = 50;

	[ExportGroup("Ports")]
	[Export]
	private Ui ui;

	private Timer reloadTimer;
	private Timer dashTimer;
	private Timer coyoteTimer;
	private bool dashTimerTimeout = true;

	private bool canShoot = true;
	private bool canDash = true;
	private bool tweenIsRunning = false;

	private float jumpVelocity;
	private float jumpGravity;
	private float fallGravity;

	private float directionX;

	//Playerparts
	private Sprite2D legSprite;
	private Sprite2D torsoSprite;
	private AnimationPlayer animationPlayer;
	private Marker2D bulletSpawner;
	private GunEnums currentGun;

	//More Parts
	private Crosshair crosshair;
	private bool controllerAim;
	private Vector2 targetDir = new Vector2(1,0);
	private Vector2 controllerAimDir;

	private Vector2 velocity;
	private Vector2 torsoPosition;
	private int torsoY = -8;

	public int Health
	{
		get => this.health;
		set
		{
			this.health = value;
			this.ui.UpdateHealth(this.health);
		}
	}

	public override void _Ready()
	{
		this.torsoSprite = this.GetNode<Sprite2D>("Sprites/TorsoSprite");
		this.legSprite = this.GetNode<Sprite2D>("Sprites/LegSprite");
		this.velocity = this.Velocity;
		this.torsoPosition = this.torsoSprite.Position;
		this.animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
		this.reloadTimer = this.GetNode<Timer>("Timer/ReloadTimer");
		this.dashTimer = this.GetNode<Timer>("Timer/DashTimer");
		this.coyoteTimer = this.GetNode<Timer>("Timer/CoyoteTimer");
		this.bulletSpawner = this.GetNode<Marker2D>("BulletSpawner");
		this.crosshair = this.GetNode<Crosshair>("Sprites/Crosshair");

		this.jumpVelocity = (-2.0f * this.jumpHeight) / this.jumpTimeToPeak;
		this.jumpGravity = (2.0f * this.jumpHeight) / (this.jumpTimeToPeak * this.jumpTimeToPeak);
		this.fallGravity = (2.0f * this.jumpHeight) / (this.jumpTimeToPeak * this.jumpTimeToDescent);

		this.reloadTimer.Timeout += this.OnReloadTimerTimeout;
		this.dashTimer.Timeout += this.OnDashTimerTimeout;

		this.ui.Init(this.health, this.health);
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.GetInput();
		this.Move(delta);
		this.Animation();
		this.MoveAndSlide();
		this.CanCoyoteJump();

	}

	public override void _Process(double delta)
	{
		this.crosshair.Update(this.targetDir, this.crosshairDistance, this.isDucking);
	}

	private void Animation()
	{
		this.torsoPosition.Y = this.torsoY;
		//Flip Sprite
		if (this.directionX != 0)
		{
			this.legSprite.FlipH = this.directionX < 0;
		}

		//Jump Animation
		if (!this.IsOnFloor())
		{
			this.animationPlayer.Play("jump");
		}
		else if (this.isDucking)
		{
			this.animationPlayer.Play("duck");
			this.torsoPosition.Y = 0;
		}
		//Run Animation
		else if (this.directionX != 0)
		{
			this.animationPlayer.Play("run");
		}
		//Idle Animation
		else
		{
			this.animationPlayer.Play("idle");
		}

		//Torso
		this.torsoSprite.Frame = GunDirections.GunDirectionDict[this.GetRoundedAimDirection()] + (int)this.currentGun * this.torsoSprite.Hframes;
		this.torsoSprite.Position = this.torsoPosition;
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		Vector2 rightStickInput = Input.GetVector("aim_left", "aim_right", "aim_up", "aim_down");

		if (rightStickInput != Vector2.Zero)
		{
			this.controllerAim = true;
			this.controllerAimDir = rightStickInput;
		}

		if (@event is InputEventMouseMotion)
		{
			this.controllerAim = false;
		}

		this.GetAimDir();
	}

	private void GetInput()
	{
		this.directionX = Input.GetAxis("left", "right");
		if (Input.IsActionJustPressed("jump") && (this.IsOnFloor() || this.coyoteTimer.TimeLeft >= 0))
		{
			this.velocity.Y = this.jumpVelocity;
		}
		if (Input.IsActionPressed("shoot") && this.canShoot)
		{
			this.reloadTimer.Start();
			this.canShoot = false;
			FX.SpawnBullet(this, this.bulletSpawner, this.targetDir, this.isDucking);
		}
		if (Input.IsActionJustPressed("dash") && this.canDash)
		{
			this.dashTimer.Start();
			this.dashTimerTimeout = false;
			this.canDash = false;
			this.tweenIsRunning = true;
			Tween tween = this.CreateTween();
			tween.TweenProperty(this, "velocity:x", this.velocity.X + this.directionX * this.dashSpeed, 0.3f);
			tween.TweenCallback(Callable.From(this.DashFinish));
		}
		this.isDucking = Input.IsActionPressed("duck") && this.IsOnFloor();
		if (Input.IsActionJustPressed("toggle"))
		{
			this.currentGun = (GunEnums)Mathf.PosMod((int)this.currentGun + 1, Enum.GetValues(typeof(GunEnums)).Length);
			GD.Print(this.currentGun);
		}
	}

	private void DashFinish()
	{
		this.velocity.X = Mathf.MoveToward(this.velocity.X, 0f, 500f);
		this.tweenIsRunning = false;
	}


	private void Move(double delta)
	{
		if (!this.IsOnFloor())
		{
			this.velocity.Y += this.GetJumpGravity() * (float)delta;
		}

		if (this.IsOnFloor())
		{
			this.velocity.Y = this.velocity.Y > 0 ? 0 : this.velocity.Y;
			if (!this.tweenIsRunning && this.dashTimerTimeout)
			{
				this.canDash = true;
			}
		}

		if (this.IsOnCeiling() && this.velocity.Y < 0)
		{
			this.velocity.Y = 0;
		}

		if (this.directionX != 0 && !this.isDucking)
		{
			this.velocity.X = (float)Mathf.MoveToward(this.velocity.X, this.directionX * this.speed, this.acceleration * delta);
		}
		else
		{
			if (this.velocity.X != 0)
			{
				this.velocity.X = (float)Mathf.MoveToward(this.velocity.X, 0, this.friction * delta);
			}
			else
			{
				this.velocity.X = 0;
			}
		}


		this.Velocity = this.velocity;
	}

	private float GetJumpGravity()
	{
		if (this.velocity.Y < 0)
		{
			return this.jumpGravity;
		}
		else
		{
			return this.fallGravity;
		}
	}

	private Vector2 GetNormalizedMousePosition()
{
    Vector2 mousePos = this.GetLocalMousePosition();
    mousePos.Y += 20f;
    return mousePos.Normalized();
}

	private Vector2I GetRoundedAimDirection()
	{
		Vector2 rawDirection = this.targetDir;
		Vector2I adjustedDirection = new Vector2I((int)Math.Round(rawDirection.X), (int)Math.Round(rawDirection.Y));
		return adjustedDirection;
	}

	private void OnReloadTimerTimeout() => this.canShoot = true;

	private void OnDashTimerTimeout() => this.dashTimerTimeout = true;

	public override void _ExitTree()
	{
		this.reloadTimer.Timeout -= this.OnReloadTimerTimeout;
	}

	public void Hit()
	{
		this.Health--;
	}

	public void Heal()
	{
		this.Health++;
	}

	private void CanCoyoteJump()
	{
		if (!this.allowedToCoyoteJump)
		{
			return;
		}

		bool isOnFloorNow = this.IsOnFloor();

		if (this.wasOnFloorLastFrame && !isOnFloorNow)
		{
			this.coyoteTimer.Start();
		}

		this.wasOnFloorLastFrame = isOnFloorNow;
	}

	private void GetAimDir()
	{
		if (this.controllerAim)
		{
			this.targetDir = this.controllerAimDir.Normalized();
		}
		else
		{
			this.targetDir = this.GetNormalizedMousePosition();
		}

	}
}
