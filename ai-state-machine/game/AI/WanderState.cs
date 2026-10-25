using Godot;
using System;

public partial class WanderState : State
{
    [Export]
    private float maxWanderRange = 6f;
    [Export]
    private float minWaitTime = 0.2f;
    [Export]
    private float maxWaitTime = 2f;
	[Export]
	private float chaseRange = 4.0f;
    
    private Vector3 homePosition;
    private bool requestNewPositionOnNextUpdate = false;

    public override void Enter()
    {
        base.Enter();
        this.homePosition = this.Controller.Position;
        
        this.NewWanderPosition();
    }

    public async override void NavigationComplete()
    {
        base.NavigationComplete();

		float waitTime = (float)GD.RandRange(this.minWaitTime, this.maxWaitTime);
		await ToSignal(GetTree().CreateTimer(waitTime), SceneTreeTimer.SignalName.Timeout);

		if (!this.Active)
		{
			return;
		}

		this.NewWanderPosition();
    }

    private void NewWanderPosition()
    {
        Vector3 position = this.homePosition + this.RandomOffset() * (float)GD.RandRange(0, this.maxWanderRange);
        this.Controller.MoveToPosition(position);
    }

    public override void Update(double delta)
    {
        base.Update(delta);
		if (this.Controller.PlayerDistance < this.chaseRange)
		{
			this.StateMachine.ChangeState("Chase");
		}
    }
}