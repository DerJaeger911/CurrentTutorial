using bullethellcourse.Scripts.Entities;
using Godot;
using System;

namespace bullethellcourse.Scripts.Bullets;

public partial class Bullet : Area2D
{
	[Export]
	private float speed = 200;
	[Export]
	private string owner;

	private Timer destroyTimer;

	public Vector2 MoveDirection { get; set; }

	[Export]
	private BulletTypeEnum bulletType = BulletTypeEnum.Arrow;

	private EntityTypeEnum entityType;

	public override void _Ready()
    {
		this.destroyTimer = this.GetNode<Timer>("DestroyTimer");

		//this.CollisionLayer = LayerMask.ProjectileLayer;
		//this.CollisionMask = LayerMask.ProjectileMask;

		this.BodyEntered += this.OnBodyEntered;
		this.destroyTimer.Timeout += this.OnDestroyTimerTimeout;
    }

	public void Init(EntityTypeEnum entityType ,BulletTypeEnum bulletType)
	{
		this.entityType = entityType;
		this.bulletType = bulletType;
	}

    public override void _Process(Double delta)
    {
		this.Translate(this.MoveDirection * this.speed * (float)delta);
		this.Rotation = this.MoveDirection.Angle();
    }

	private void OnBodyEntered(Node body)
	{
		//Logic
		this.QueueFree();
	}

	private void OnDestroyTimerTimeout()
	{
		this.Visible = false;
	}

	private void OnVisibilityChanged()
	{
		if(this.Visible && this.destroyTimer != null)
		{
			destroyTimer.Start();
		}
	}
}
