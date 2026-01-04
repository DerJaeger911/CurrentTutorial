using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Pools;
using bullethellcourse.Scripts.Statics;
using Godot;
using System;

namespace bullethellcourse.Scripts.Entities;

public partial class Player : Entity
{

	[Export]
	private float braking = 0.15f;
	[Export]
	private int maxHp = 50;

	private Vector2 moveInput;

	private PlayerBulletPool playerBulletPool = new();

	protected override int MaxHp
	{
		get => this.maxHp;
		set => this.maxHp = value;
	}

	protected override Color FlashColor => Colors.Blue;

    protected override Single moveWobbleAmount => 3;

	protected override BulletPool BulletPool => this.playerBulletPool;

	public override EntityTypeEnum BulletOwner => EntityTypeEnum.Player;

	public override void _Ready()
	{
		base._Ready();
		this.CollisionLayer = LayerMask.PlayerLayer;
	}

	public override void _Process(Double delta)
	{
		base._Process(delta);

		if (Input.IsActionPressed("shoot")) 
		{
			this.Shoot(BulletTypeEnum.Arrow);
		}
	}

	public override void _PhysicsProcess(Double delta)
	{
		base._PhysicsProcess(delta);

		this.moveInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (this.moveInput.Length() > 0)
		{
			this.Velocity = this.Velocity.Lerp(this.moveInput * this.MaxSpeed, this.acceleration);
			this.isMoving = true;
		}
		else
		{
			this.Velocity = this.Velocity.Lerp(Vector2.Zero, this.braking);
			this.isMoving = false;
		}
		this.MoveAndSlide();
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

	protected override void Die()
	{
		GD.Print("Gamel over");
	}
}