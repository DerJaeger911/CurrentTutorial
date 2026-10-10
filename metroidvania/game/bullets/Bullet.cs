using Godot;
using Metroidvania.game.interfaces;
using Metroidvania.game.utility;
using System;
using System.Collections.Generic;

public partial class Bullet : Area2D
{
	private Vector2 direction;
	private float speed = 200f;
	private GunEnums gunType;
	private Node2D parent;

	private Sprite2D sprite;

	private readonly Dictionary<BulletEnums, Texture2D> bulletTextures = new()
	{
		{BulletEnums.Default, GD.Load<Texture2D>("res://assets/graphics/fire/default.png")},
		{BulletEnums.Large, GD.Load<Texture2D>("res://assets/graphics/fire/large.png")},
	};

	private CollisionShape2D collisionShape;

	public override void _Ready()
	{
		this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
		this.sprite = this.GetNode<Sprite2D>("Sprite2D");
	// 	this.collisionShape.Disabled = true;
	// 	this.GetTree().CreateTimer(0.2f).Timeout += () =>
	// {
	// 	this.collisionShape.Disabled = false;
	// };
	}

	public void Setup(Node2D parent, Vector2 direction, GunEnums gunType, Vector2 spawnOffset)
	{
		this.Position = spawnOffset;
		this.direction = direction;
		this.parent = parent;
		this.gunType = gunType;
		this.sprite.Texture = this.bulletTextures[this.BulletMatching(gunType)];

		this.BodyEntered += this.OnBulletHit;
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.Position += this.direction * this.speed * (float)delta;
	}

	private void OnBulletHit(Node body)
	{
		if (body == this.parent)
		{
			return;
		}

		if (body is IDamageable entity)
		{
			entity.Hit();
		}

		if (this.gunType == GunEnums.Rocket)
		{
			FX.SpawnExplosion(this);
		}

		this.QueueFree();
	}

	private BulletEnums BulletMatching(GunEnums gunType)
	{
		BulletEnums bulletType = gunType switch
		{
			GunEnums.Single or GunEnums.Shotgun => BulletEnums.Default,
			GunEnums.Rocket => BulletEnums.Large,
			_ => BulletEnums.Default,
		};

		return bulletType;
	}
}
