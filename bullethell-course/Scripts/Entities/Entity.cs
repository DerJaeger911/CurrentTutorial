using bullethellcourse.Scripts.Bullets;
using Godot;

namespace bullethellcourse.Scripts.Entities;

public abstract partial class Entity : CharacterBody2D
{
	[Export]
	protected float maxSpeed = 100;
	[Export]
	protected float acceleration = 0.2f;
	[Export]
	protected double shootRate = 0.1f;

	protected double lastShootTime;
	
	protected Node2D muzzle;

	private Bullet bullet;

	protected abstract BulletPool bulletPool {  get; }

	protected Sprite2D sprite;

	protected abstract EntityTypeEnum entityType { get; }

    public override void _Ready()
    {
		this.muzzle = this.GetNode<Node2D>("Muzzle");
		this.sprite = this.GetNode<Sprite2D>("Sprite");
	}

	protected void Shoot(BulletTypeEnum bulletType)
	{
		if (Time.GetUnixTimeFromSystem() - lastShootTime > shootRate)
		{
			this.lastShootTime = Time.GetUnixTimeFromSystem();

			this.InstantiateBullet(bulletType);

			this.bullet.MoveDirection = this.BulletDirection();
		}
	}

	private void InstantiateBullet(BulletTypeEnum bulletType)
	{
		Node parent = this.GetTree().CurrentScene;
		this.bullet = (Bullet)this.bulletPool.Spawn(parent);
		this.bullet.Init(this.entityType, bulletType);
		this.bullet.GlobalPosition = this.muzzle.GlobalPosition;
	}

	protected abstract Vector2 BulletDirection();

	protected abstract void FlipH();
}
