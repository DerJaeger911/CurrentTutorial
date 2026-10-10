using Godot;
using Metroidvania.game.interfaces;
using Metroidvania.game.utility;
using System.Linq;

public partial class Soldier : CharacterBody2D, IDamageable, IEnemy
{
	private Vector2I direction = Vector2I.Right;
	private int health = 2;
	private int speed = 20;
	private Sprite2D sprite;
	private Area2D floorLeftArea;
	private Area2D floorRightArea;
	private AnimationPlayer animationPlayer;
	private Area2D playerDetectionArea;
	private Player player;
	private Timer shootTimer;
	private Marker2D bulletSpawner;


	private EnemySpawner enemySpawner;


	public override void _Ready()
	{
		this.floorLeftArea = this.GetNode<Area2D>("FloorLeftArea");
		this.floorRightArea = this.GetNode<Area2D>("FloorRightArea");
		this.playerDetectionArea = this.GetNode<Area2D>("PlayerArea");
		this.sprite = this.GetNode<Sprite2D>("Sprite2D");
		this.animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
		this.shootTimer = this.GetNode<Timer>("ShootTimer");
		this.bulletSpawner = this.GetNode<Marker2D>("BulletSpawner");

		this.floorLeftArea.BodyExited += this.OnFloorLeftExit;
		this.floorRightArea.BodyExited += this.OnFloorRightExit;
		this.playerDetectionArea.BodyEntered += this.OnPlayerDetected;
		this.playerDetectionArea.BodyExited += this.OnPlayerLost;
		this.shootTimer.Timeout += this.OnShootTimerTimeout;
	}

	public void Setup(EnemySpawner enemySpawner)
	{
		this.GlobalPosition = enemySpawner.GlobalPosition;
		this.enemySpawner = enemySpawner;
	}

	public override void _PhysicsProcess(double delta)
	{
		this.Move();
		this.Animation();
	}

	private void Animation()
	{
		this.sprite.FlipH = this.direction.X < 0;

		if (this.direction != Vector2I.Zero)
		{
			this.animationPlayer.Play("run");
		}
		if (this.player != null)
		{
			Vector2 positionDifference = this.player.GlobalPosition - this.GlobalPosition;
			this.sprite.FlipH = positionDifference.X < 0;
			if (positionDifference.Y > -60)
			{
				this.animationPlayer.Play("shoot_h");
			}
			else
			{
				this.animationPlayer.Play("shoot_up");
			}
		}
	}

	private void Move()
	{
		int moveFactor = (this.player == null) ? 1 : 0;
		this.Velocity = this.direction * this.speed * moveFactor;

		this.MoveAndSlide();
	}

	private void OnFloorLeftExit(Node2D body)
	{
		this.direction = Vector2I.Right;
	}

	private void OnFloorRightExit(Node2D body)
	{
		this.direction = Vector2I.Left;
	}

	private void OnPlayerDetected(Node body)
	{
		if (body is Player player)
		{
			this.player = player;
			this.shootTimer.Start();
		}
	}

	private void OnPlayerLost(Node body)
	{
		if (body is Player player && this.player == player)
		{
			this.player = null;
		}
	}

	private void OnShootTimerTimeout()
	{
		if(this.player != null)
		{
			var shootDirection = (this.player.GlobalPosition - this.GlobalPosition).Normalized();
			FX.SpawnBullet(this, this.bulletSpawner, shootDirection, false, GunEnums.Single);
			
		}
	}

	public void Hit()
	{
		this.health--;
		if (this.health <= 0)
		{
			this.Die();
			return;
		}

		var shaderMaterial = this.sprite.Material as ShaderMaterial;

		if (shaderMaterial != null)
		{
			shaderMaterial.SetShaderParameter("ColorParameter", new Color(1, 0, 0));
			Tween tween = this.CreateTween();
			tween.TweenProperty(shaderMaterial, "shader_parameter/Progress", 0.7, 0.1);
			tween.TweenProperty(shaderMaterial, "shader_parameter/Progress", 0.0, 0.2);
		}
		else
		{
			GD.PrintErr($"{this.Name} has no Shadermaterial!");
		}
	}

	private void Die()
	{
		this.GetChildren().OfType<Area2D>().ToList().ForEach(area => area.Monitoring = false);
		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		// this.floorLeftArea.BodyExited -= this.OnFloorLeftExit;
		// this.floorRightArea.BodyExited -= this.OnFloorRightExit;
		// this.playerDetectionArea.BodyEntered -= this.OnPlayerDetected;
		// this.playerDetectionArea.BodyExited -= this.OnPlayerLost;
		// this.shootTimer.Timeout -= this.OnShootTimerTimeout;

		this.shootTimer.Stop();

		this.player = null;
		this.SetPhysicsProcess(false);

		if (this.enemySpawner != null)
		{
			this.enemySpawner.IsDefeated = true;
		}

		this.animationPlayer.CurrentAnimation = "death";
	}
}
