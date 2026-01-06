using Godot;

namespace Dcozysandbox.Scripts.Player;

public partial class Player : Entity
{
    private AnimationTree animationTree;

    public override void _Ready()
    {
        base._Ready();
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
    }

    protected override void GetDirection()
    {
        this.direction = Input.GetVector("left", "right", "up", "down");
    }

	protected override void SetAnimation()
    {
        GD.Print(this.direction);

        this.animationTree.Set("parameters/MoveStateMachine/idle/blend_position", this.direction);
    }
}
