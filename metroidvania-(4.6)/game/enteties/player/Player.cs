using Godot;
using Metroidvania.game;
using Metroidvania.game.interfaces;
using Metroidvania.game.utility;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D, IDamageable
{
	[ExportGroup("Movement")]
	[Export]
	private float speed = 120f;
	[Export]
	private float acceleration = 600f;
	[Export]
	private float friction = 600f;
	[ExportGroup("Jumping")]
	[Export]
	private float jumpHeight = 100f;
	[Export]
	private float jumpTimeToPeak = 0.5f;
	[Export]
	private float jumpTimeToDescent = 0.4f;
	[Export]
	private int health = 3;

	private Timer reloadTimer;
	private bool canShoot = true;

	private float jumpVelocity;
	private float jumpGravity;
	private float fallGravity;

	private float directionX;

	private Sprite2D legSprite;
	private Sprite2D torsoSprite;
	private AnimationPlayer animationPlayer;
	private Marker2D bulletSpawner;

	private Vector2 velocity;

	public override void _Ready()
	{
		this.torsoSprite = this.GetNode<Sprite2D>("Sprites/TorsoSprite");
		this.legSprite = this.GetNode<Sprite2D>("Sprites/LegSprite");
		this.velocity = this.Velocity;
		this.animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
		this.reloadTimer = this.GetNode<Timer>("Timer/ReloadTimer");
		this.bulletSpawner = this.GetNode<Marker2D>("BulletSpawner");

		this.jumpVelocity = (-2.0f * this.jumpHeight) / this.jumpTimeToPeak;
		this.jumpGravity = (2.0f * this.jumpHeight) / (this.jumpTimeToPeak * this.jumpTimeToPeak);
		this.fallGravity = (2.0f * this.jumpHeight) / (this.jumpTimeToPeak * this.jumpTimeToDescent);

		this.reloadTimer.Timeout += this.OnReloadTimerTimeout;
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.GetInput();
		this.Move(delta);
		this.Animation();
		this.MoveAndSlide();
	}

	private void Animation()
	{
		if (this.directionX != 0)
		{
			this.legSprite.FlipH = this.directionX < 0;
		}

		if (!this.IsOnFloor())
		{
			this.animationPlayer.Play("jump");
		}
		else if (this.directionX != 0)
		{
			this.animationPlayer.Play("run");
		}
		else
		{
			this.animationPlayer.Play("idle");
		}

		this.torsoSprite.Frame = GunDirections.GunDirectionDict[this.GetMoouseDirection()];
	}

	private void GetInput()
	{
		this.directionX = Input.GetAxis("left", "right");
		if (Input.IsActionJustPressed("jump") && this.IsOnFloor())
		{
			this.velocity.Y = this.jumpVelocity;
		}
		if (Input.IsActionPressed("shoot") && this.canShoot)
		{
			this.reloadTimer.Start();
			this.canShoot = false;
			FX.SpawnBullet(this, this.bulletSpawner, this.CorrectedLocalMoussePosition());
		}
	}

	private void Move(double delta)
	{
		if (!this.IsOnFloor())
		{
			this.velocity.Y += this.GetJumpGravity() * (float)delta;
		}

		if (this.IsOnFloor() && this.velocity.Y > 0)
		{
			this.velocity.Y = 0;
		}

		if (this.IsOnCeiling() && this.velocity.Y < 0)
		{
			this.velocity.Y = 0;
		}

		if (this.directionX != 0)
		{
			this.velocity.X = (float)Mathf.MoveToward(this.velocity.X, this.directionX * this.speed, this.acceleration * delta);
		}
		else
		{
			this.velocity.X = (float)Mathf.MoveToward(this.velocity.X, 0, this.friction * delta);
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

	private Vector2I GetMoouseDirection()
	{
		Vector2 rawDirection = this.GetLocalMousePosition().Normalized();
		Vector2I adjustedDirection = new Vector2I((int)Math.Round(rawDirection.X), (int)Math.Round(rawDirection.Y));
		return adjustedDirection;
	}

	private void OnReloadTimerTimeout() => this.canShoot = true;

	public override void _ExitTree()
	{
		this.reloadTimer.Timeout -= this.OnReloadTimerTimeout;
	}

	private Vector2 CorrectedLocalMoussePosition()
	{
		Vector2 rawDirection = this.GetLocalMousePosition();
		rawDirection.Y -= this.bulletSpawner.Position.Y;
		Vector2 correctedDirection = rawDirection.Normalized();
		return correctedDirection;
	}

	public void Hit()
	{
		this.health--;
		GD.Print($"Health left: {this.health}");
	}
}
