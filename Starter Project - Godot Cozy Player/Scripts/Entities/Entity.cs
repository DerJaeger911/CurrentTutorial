using Dcozysandbox.Scripts.Helper;
using Godot;

namespace Dcozysandbox.Scripts.Entities;

public abstract partial class Entity : CharacterBody2D
{
	[Export]
	public virtual float Speed { get; set; } = 200;
	[Export]
	protected virtual int MaxHealth { get; set; } = 5;

	protected int Health { get; set; }

	protected Vector2 Direction { get; set; }

	protected Vector2 PushDirection { get; set; }
	protected System.Boolean CanMove { get; set; } = true;



	public override void _Ready()
	{
		base._Ready();

		this.Health = this.MaxHealth;
	}

	public override void _PhysicsProcess(System.Double delta)
	{
		this.GetDirection();


		this.SetAnimation();
		if (this.CanMove)
		{
			this.Velocity = this.Direction * this.Speed + this.PushDirection;
		}
		else
		{
			this.Velocity = Vector2.Zero;
		}
			this.MoveAndSlide();
	}

	protected abstract void GetDirection();

	protected abstract void SetAnimation();
}
