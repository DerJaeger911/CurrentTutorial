using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Statics;
using Godot;
using System;

namespace bullethellcourse.Scripts.Entities;

public partial class Player : Entity
{

	[Export]
	private float braking = 0.15f;

	private Vector2 moveInput;

	private PlayerBulletPool playerBulletPool = new();

    protected override BulletPool bulletPool => this.playerBulletPool;

	protected override EntityTypeEnum entityType => EntityTypeEnum.Player;

	public override void _Ready()
	{
		base._Ready();
		this.CollisionLayer = LayerMask.PlayerLayer;
		this.CollisionMask = LayerMask.EntityMask;
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.moveInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (this.moveInput.Length() > 0)
		{
			this.Velocity = this.Velocity.Lerp(this.moveInput * this.maxSpeed, this.acceleration);
		}
		else
		{
			this.Velocity = this.Velocity.Lerp(Vector2.Zero, this.braking);
		}
		this.MoveAndSlide();
	}

    public override void _Process(Double delta)
    {
		this.FlipH();

		if (Input.IsActionPressed("shoot")) 
		{
			this.Shoot(BulletTypeEnum.Arrow);
		}
    }

    protected override void FlipH()
    {
		this.sprite.FlipH = this.GetGlobalMousePosition().X > this.GlobalPosition.X;
	}

	protected override Vector2 BulletDirection()
	{
		var mousePosition = this.GetGlobalMousePosition();

		var shootDirection = this.muzzle.GlobalPosition.DirectionTo(mousePosition);

		return shootDirection;
	}
}