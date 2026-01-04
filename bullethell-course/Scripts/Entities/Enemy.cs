using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Pools;
using bullethellcourse.Scripts.Statics;
using Godot;

namespace bullethellcourse.Scripts.Entities;

internal partial class Enemy : Entity
{
	[Export]
	private float drag;
	[Export]
	private float stopRange;
	[Export]
	private float avoidanceTargetDistance = 80;
	[Export]
	private float shootRange = 300;
	[Export]
	private int maxHp = 5;
	[Export]
	private bool isFlipped;
	protected override int MaxHp
	{
		get => this.maxHp;
		set => this.maxHp = value;
	}

	protected virtual BulletTypeEnum BulletType { get; } = BulletTypeEnum.Fire;

	private RayCast2D avoidanceRay;

	private Player player;
	private float playerDistance;
	private Vector2 playerDirection;

	private EnemyBulletPool enemyBulletPool = new();

	protected override Color FlashColor => Colors.Red;

    protected override System.Single moveWobbleAmount => 2;

	protected override BulletPool BulletPool => this.enemyBulletPool;

	public override EntityTypeEnum BulletOwner => EntityTypeEnum.Enemy;

	private bool isVisibilityConnected;

	public override void _Ready()
	{
		base._Ready();
		if (!this.isVisibilityConnected)
		{
			this.VisibilityChanged += this.OnVisibilityChanged;
			this.isVisibilityConnected = true;
		}
		this.CollisionLayer = LayerMask.EnemyLayer;
		this.player = this.GetTree().GetFirstNodeInGroup("Player") as Player;
		this.avoidanceRay = this.GetNode<RayCast2D>("AvoidanceRay");
	}

	public void ResetFailSafe()
	{
		this.Visible = true;
		this.SetProcess(true);
		this.SetPhysicsProcess(true);
		this.ResetHealth();
		this.isMoving = false;
		this.Velocity = Vector2.Zero;
	}

	public override void _Process(System.Double delta)
	{
		base._Process(delta);

		this.playerDistance = this.GlobalPosition.DistanceTo(this.player.GlobalPosition);
		this.playerDirection = this.GlobalPosition.DirectionTo(this.player.GlobalPosition);

		if(this.playerDistance < this.shootRange)
		{
			this.Shoot(this.BulletType);
		}
	}

	public override void _PhysicsProcess(System.Double delta)
	{
		base._PhysicsProcess(delta);

		var moveDirection = this.playerDirection;
		var localAvoidance = this.LocalAvoidance();

		if(localAvoidance.Length() > 0)
		{
			moveDirection = localAvoidance;
		}

		if (this.Velocity.Length() < this.MaxSpeed && this.playerDistance > this.stopRange)
		{
			this.Velocity += moveDirection * this.acceleration;
			this.isMoving = true;
		}
		else
		{
			this.Velocity *= this.drag;
			this.isMoving = false;
		}

		this.MoveAndSlide();
	}

	protected override Vector2 BulletDirection()
	{
		var shootDirection = this.GlobalPosition.DirectionTo(this.player.GlobalPosition);
		return shootDirection;
	}

	protected override void FlipH()
	{
		if (!this.isFlipped)
		{
			this.sprite.FlipH = this.playerDirection.X > 0;
		}
		else
		{
			this.sprite.FlipH = this.playerDirection.X > 0;
		}
	}

	protected override void Die()
	{
		this.Visible = false; 
		this.CallDeferred(nameof(this.RemoveFromParent));
		this.Disconnect();
	}

	private void RemoveFromParent()
	{
		this.GetParent()?.RemoveChild(this);
		
	}

	private Vector2 LocalAvoidance()
	{
		this.avoidanceRay.TargetPosition = this.ToLocal(this.player.GlobalPosition).Normalized();
		this.avoidanceRay.TargetPosition *= this.avoidanceTargetDistance;
		var obstacle = this.avoidanceRay.GetCollider();
		if (!this.avoidanceRay.IsColliding() || obstacle == this.player)
		{
			return Vector2.Zero;
		}

		var obstaclePoint = this.avoidanceRay.GetCollisionPoint();
		var obstacleDirection = this.GlobalPosition.DirectionTo(obstaclePoint);

		return new Vector2(-obstacleDirection.Y, obstacleDirection.X);
	}

	private void OnVisibilityChanged()
	{
		if (this.Visible)
		{
			this.SetProcess(true);
			this.SetPhysicsProcess(true);
			this.ResetHealth();
		}
		else
		{
			this.SetProcess(false);
			this.SetPhysicsProcess(false);
			this.GlobalPosition = new Vector2(0, 99999);
		}
	}

	private void Disconnect()
	{
		if (this.isVisibilityConnected)
		{
			this.VisibilityChanged -= this.OnVisibilityChanged;
			this.isVisibilityConnected = false;
		}
	}

	public override void _ExitTree()
	{
		this.Disconnect();
	}
}
