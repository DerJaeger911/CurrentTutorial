using Godot;
using RoguelikeCourse.Scripts.Entities.Bases;
using RoguelikeCourse.Scripts.Statics;
using System;

namespace RoguelikeCourse.Scripts;

public partial class Player : Entity, IPlayer
{

	[Export]
	private double shootRate = 0.4;
	private double lastShootTime;

	private PackedScene projectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/projectile.tscn");

	private Sprite2D sprite;
	private Node2D weaponOrigin;
	private Node2D muzzle;

	public override void _Ready()
	{
		this.CollisionLayer = LayerMasks.PlayerLayer;
		this.CollisionMask = LayerMasks.PlayerMasks;
		this.AddToGroup("Player");
		this.sprite = this.GetNode<Sprite2D>("Sprite");
		this.weaponOrigin = this.GetNode<Node2D>("Weapon");
		this.muzzle = this.GetNode<Node2D>("Weapon/Muzzle");

		this.MoveSpeed = 100;
		this.MaxHp = 4;

		base._Ready();
    }

	public override void _PhysicsProcess(Double delta)
	{
		Vector2 moveInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		this.Velocity = moveInput * this.MoveSpeed;
		this.MoveAndSlide();
	}

	public override void _Process(Double delta)
	{
		Vector2 mousePosition = this.GetGlobalMousePosition();
		Vector2 mouseDirection = (mousePosition - this.GlobalPosition).Normalized();
		this.weaponOrigin.RotationDegrees = Mathf.RadToDeg(mouseDirection.Angle()) + 90;

		this.sprite.FlipH = mouseDirection.X > 0;

		if (Input.IsActionPressed("attack"))
		{
			if(Time.GetUnixTimeFromSystem() - this.lastShootTime > this.shootRate)
			{
				this.Shoot();
			}
		}
	}

	private void Shoot()
	{
		this.lastShootTime = Time.GetUnixTimeFromSystem();

		Projectile projectile = this.projectileScene.Instantiate<Projectile>();
		this.GetTree().Root.AddChild(projectile);
		projectile.GlobalPosition = this.muzzle.GlobalPosition;
		projectile.Rotation = this.weaponOrigin.Rotation;
		projectile.ownerCharacter = this;
	}
    protected override void Die()
    {

    }
}