using bullethellcourse.Scripts.Bullets;
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
	private float shootRange;

	private RayCast2D avoidanceRay;

	private Player player;
	private float playerDistance;
	private Vector2 playerDirection;

	private EnemyBulletPool enemyBulletPool = new();

	protected override BulletPool bulletPool => this.enemyBulletPool;

	protected override EntityTypeEnum entityType => EntityTypeEnum.Enemy;

	public override void _Ready()
	{
		base._Ready();
		this.player = this.GetTree().GetFirstNodeInGroup("Player") as Player;
		this.avoidanceRay = this.GetNode<RayCast2D>("AvoidanceRay");
	}

	public override void _Process(System.Double delta)
	{
		this.FlipH();

		playerDistance = this.GlobalPosition.DistanceTo(this.player.GlobalPosition);
		playerDirection = this.GlobalPosition.DirectionTo(this.player.GlobalPosition);

		if(this.playerDistance < this.shootRange)
		{
			this.Shoot(BulletTypeEnum.Fire);
		}
	}

	public override void _PhysicsProcess(System.Double delta)
	{
		var moveDirection = this.playerDirection;
		var localAvoidance = this.LocalAvoidance();

		if(localAvoidance.Length() > 0)
		{
			moveDirection = localAvoidance;
		}

        if (this.Velocity.Length() < this.maxSpeed && this.playerDistance > this.stopRange)
        {
            this.Velocity += moveDirection * this.acceleration;
        }
        else
        {
            this.Velocity *= this.drag;
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
		this.sprite.FlipH = this.playerDirection.X > 0;
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
}
