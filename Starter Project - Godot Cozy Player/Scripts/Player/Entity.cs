using Dcozysandbox.Scripts.AutoLoads.Busses;
using Godot;

namespace Dcozysandbox.Scripts.Player;

public abstract partial class Entity : CharacterBody2D
{
	[Export]
	private float speed = 200;

	protected Vector2 direction;

    protected bool action;

	protected bool canMove = true;

	public override void _PhysicsProcess(System.Double delta)
    {
		if (this.canMove)
		{
			this.GetInput();
		}
        this.SetAnimation();
		this.Velocity = this.direction * this.speed * (this.canMove ? 1 : 0);
		this.MoveAndSlide();
    }

    protected abstract void GetInput();

    protected abstract void SetAnimation();
}
