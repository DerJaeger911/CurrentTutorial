using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Statics;
using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export]
	private float speed = 200;
	[Export]
	private string owner;

	private Timer destroyTimer;

	private Vector2 moveDirection;

	[Export]
	private BulletTypeEnum bulletType = BulletTypeEnum.Arrow;

	public override void _Ready()
    {
		this.destroyTimer = this.GetNode<Timer>("DestroyTimer");

		//this.CollisionLayer = LayerMask.ProjectileLayer;
		//this.CollisionMask = LayerMask.ProjectileMask;

		this.BodyEntered += this.OnBodyEntered;
		this.destroyTimer.Timeout += this.OnDestroyTimerTimeout;
    }

	public void Init(BulletTypeEnum bulletType)
	{
		this.bulletType = bulletType;
	}

    public override void _Process(Double delta)
    {
		this.Translate(this.moveDirection * this.speed * (float)delta);
    }

	private void OnBodyEntered(Node body)
	{
		//Logic
		this.QueueFree();
	}

	private void OnDestroyTimerTimeout()
	{
		this.QueueFree();
	}
}
