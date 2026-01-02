using bullethellcourse.Scripts.Bullets;
using Godot;

namespace bullethellcourse.Scripts.Entities;

public abstract partial class Entity : CharacterBody2D
{
	[Export]
	protected double shootRate = 0.1f;

	protected double lastShootTime;
	
	protected Node2D muzzle;

	protected PackedScene bulletScene;

	private Bullet bullet;

	protected abstract BulletPool bulletPool {  get; }

	protected abstract EntityTypeEnum EntityType { get; }

    public override void _Ready()
    {
		this.muzzle = this.GetNode<Node2D>("Muzzle");

		this.bulletScene = GD.Load<PackedScene>("res://Scenes/bullet.tscn");
	}

	protected void Shoot(BulletTypeEnum bulletType)
	{
		this.lastShootTime = Time.GetUnixTimeFromSystem();

		this.InstantiateBullet(bulletType);

		this.bullet.MoveDirection = this.BulletDirection();

		
	}

	private void InstantiateBullet(BulletTypeEnum bulletType)
	{
		this.bullet = (Bullet)this.bulletPool.Spawn();
		bullet.Init(this.EntityType ,bulletType);
		this.GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, bullet);
		bullet.GlobalPosition = this.muzzle.GlobalPosition;
	}

	protected abstract Vector2 BulletDirection();
}
