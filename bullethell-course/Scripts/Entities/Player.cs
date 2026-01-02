using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Statics;
using Godot;
using System;

namespace bullethellcourse.Scripts.Entities;

public partial class Player : Entity
{
	[Export]
	private float maxSpeed = 100;
	[Export]
	private float acceleration = 0.2f;
	[Export]
	private float braking = 0.15f;

	private Vector2 moveInput;

	private Sprite2D sprite;

	private PlayerBulletPool playerBulletPool = new();

    protected override BulletPool bulletPool => this.playerBulletPool;

	protected override EntityTypeEnum EntityType => EntityTypeEnum.Player;

	public override void _Ready()
	{
		base._Ready();
		this.sprite = this.GetNode<Sprite2D>("Sprite");
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
        this.sprite.FlipH = this.GetGlobalMousePosition().X >this.GlobalPosition.X;

		if (Input.IsActionPressed("shoot")) 
		{
			if(Time.GetUnixTimeFromSystem() - lastShootTime > shootRate)
			{
				this.Shoot(BulletTypeEnum.Arrow);
			}
		}
    }

	protected override Vector2 BulletDirection()
	{
		var mousePosition = this.GetGlobalMousePosition();

		var shootDirection = this.muzzle.GlobalPosition.DirectionTo(mousePosition);

		return shootDirection;
	}
}