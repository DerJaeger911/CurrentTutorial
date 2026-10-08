using Godot;
using Metroidvania.game.interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class Bullet : Area2D
{
	private Vector2 direction;
	private float speed = 200f;
	private const int offset = 16;
	private Node2D parent;

	private CollisionShape2D collisionShape;

	public override void _Ready()
	{
		this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
	// 	this.collisionShape.Disabled = true;
	// 	this.GetTree().CreateTimer(0.2f).Timeout += () =>
	// {
	// 	this.collisionShape.Disabled = false;
	// };
	}

	public void Setup(Node2D parent, Vector2 position, Vector2 direction)
	{
		this.Position = position + direction * offset;
		this.direction = direction;
		this.parent = parent;

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

		this.QueueFree();
	}
}
