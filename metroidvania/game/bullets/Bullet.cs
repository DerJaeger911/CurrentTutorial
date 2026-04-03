using Godot;
using Metroidvania.game.interfaces;
using System;
using System.Runtime.CompilerServices;

public partial class Bullet : Area2D
{
	private Vector2 direction;
	private float speed = 200f;
	private const int offset = 16;

	public void Setup(Vector2 position, Vector2 direction)
	{
		position = new Vector2(position.X, position.Y - 20);
		this.Position = position + direction * offset;
		this.direction = direction;

		this.BodyEntered += this.OnBulletHit;
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.Position += this.direction * this.speed * (float)delta;
	}

	private void OnBulletHit(Node body)
	{
		if (body is IDamageable entity)
		{
			entity.Hit();
		}
	}
}
