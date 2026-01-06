using Godot;

namespace Dcozysandbox.Scripts.Player;

public abstract partial class Entity : CharacterBody2D
{
	[Export]
	private float speed = 200;

	protected Vector2 direction;

    public override void _PhysicsProcess(System.Double delta)
    {
        this.GetDirection();
        this.SetAnimation();
		this.Velocity = this.direction * this.speed;
        this.MoveAndSlide();
    }

    protected abstract void GetDirection();

    protected abstract void SetAnimation();
}
