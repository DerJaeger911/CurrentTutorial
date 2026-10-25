using Godot;

public partial class FleeState : State
{
    [Export]
    private float fleeDistance = 6f;

    public override void Enter()
    {
        base.Enter();
        this.Controller.IsRunning = true;
        

        this.Controller.IsStopped = false;

        this.Flee();
    }

    public override void Exit()
    {
        base.Exit();
        this.Controller.IsRunning = false;
    }

    private void Flee()
    {
        Vector3 playerDir = (this.Controller.Position - this.Controller.PlayerCharacter.Position).Normalized();
        var movePosition = this.Controller.Position + (playerDir * this.fleeDistance);
        this.Controller.MoveToPosition(movePosition);
    }

    public override void NavigationComplete()
    {
        base.NavigationComplete();
        this.StateMachine.ChangeState("Wander");
    }
}