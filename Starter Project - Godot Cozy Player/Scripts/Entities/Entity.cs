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

		GD.Print(this.MaxHealth);

		this.Health = this.MaxHealth;
    }

	public override void _PhysicsProcess(System.Double delta)
    {
		if (this.CanMove)
		{
			this.GetDirection();
		}

        this.SetAnimation();
		this.Velocity = this.Direction * this.Speed + this.PushDirection;
		GD.Print(this.Velocity);
		this.MoveAndSlide();
    }

	protected abstract void GetDirection();

    protected abstract void SetAnimation();
}
