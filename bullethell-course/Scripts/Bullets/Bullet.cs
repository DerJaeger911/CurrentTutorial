using bullethellcourse.Scripts.Entities;
using bullethellcourse.Scripts.Statics;
using Godot;
using System;

namespace bullethellcourse.Scripts.Bullets;

public partial class Bullet : Area2D
{
	[Export]
	private float speed = 200;

	private Timer destroyTimer;

	public float AdditionalSpeed { get; set; }

	public Vector2 MoveDirection { get; set; }

	[Export]
	private BulletTypeEnum bulletType = BulletTypeEnum.Arrow;

	private EntityTypeEnum owner;

	public override void _Ready()
    {
		this.destroyTimer = this.GetNode<Timer>("DestroyTimer");

		this.CollisionLayer = LayerMask.ProjectileLayer;
		this.CollisionMask = LayerMask.ProjectileMask;

		this.BodyEntered += this.OnBodyEntered;
		this.destroyTimer.Timeout += this.OnDestroyTimerTimeout;
    }

	public void Init(EntityTypeEnum entityType ,BulletTypeEnum bulletType)
	{
		this.owner = entityType;
		this.bulletType = bulletType;
	}

    public override void _Process(Double delta)
    {
		if (!this.Visible)
		{
			return;
		}
		this.Translate(this.MoveDirection * (this.speed + this.AdditionalSpeed) * (float)delta);
		this.Rotation = this.MoveDirection.Angle();
    }

	private void OnBodyEntered(Node body)
	{
		if (body is Entity entity)
		{
			if(entity.BulletOwner == this.owner)
			{
				return;
			}

			entity.TakeDamage(1);
		}
		this.Visible = false;
		this.CallDeferred(nameof(this.RemoveFromParent));
	}

	private void OnDestroyTimerTimeout()
	{
		this.Visible = false;
		this.RemoveFromParent();
	}

	private void RemoveFromParent()
	{
		this.GetParent()?.RemoveChild(this);
	}

	private void OnVisibilityChanged()
	{
		if(this.Visible && this.destroyTimer != null)
		{
            this.destroyTimer.Start();
			this.AdditionalSpeed = 0;
		}
	}
}
