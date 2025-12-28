using Godot;
using RoguelikeCourse.Scripts.Entities.Bases;
using RoguelikeCourse.Scripts.Manager.Signals;
using RoguelikeCourse.Scripts.Statics;
using System;

namespace RoguelikeCourse.Scripts;

public partial class Player : Entity, IPlayer
{

	[Export]
	private double shootRate = 0.4;
	private double initialShootRate;
	private double lastShootTime;

	private PackedScene projectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/projectile.tscn");

	private Sprite2D sprite;
	private Node2D weaponOrigin;
	private Node2D muzzle;

    public double ShootRate { get => this.shootRate; set => this.shootRate = value; }

    public override void _Ready()
	{
        base._Ready();
        this.CollisionLayer = LayerMasks.PlayerLayer;
		this.AddToGroup("Player");
		this.sprite = this.GetNode<Sprite2D>("Sprite");
		this.weaponOrigin = this.GetNode<Node2D>("Weapon");
		this.muzzle = this.GetNode<Node2D>("Weapon/Muzzle");
		this.MoveSpeed = 100;
		this.MaxHp = 4;
		this.initialShootRate = this.shootRate;
		GameSignals.Instance.EmitSignal(nameof(GameSignals.PlayerUpdateHealth), this.CurrentHp, this.MaxHp);
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
			if(Time.GetUnixTimeFromSystem() - this.lastShootTime > this.ShootRate)
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

    public override void TakeDamage(int amaount, Node target)
    {
        base.TakeDamage(amaount, target);

		GameSignals.Instance.EmitSignal(nameof(GameSignals.PlayerUpdateHealth), this.CurrentHp, this.MaxHp);
	}

    public override void Heal(int amount)
    {
        base.Heal(amount);

		GameSignals.Instance.EmitSignal(nameof(GameSignals.PlayerUpdateHealth), this.CurrentHp, this.MaxHp);
	}

	public void InternalStatResets(StatEnum stat)
	{
		switch (stat)
		{
			case StatEnum.ShootRate:
				this.ShootRate = this.initialShootRate;
			break;
		}
	}

    protected override void Die()
    {
    }
}