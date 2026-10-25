using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
	[Export]
	private State defaultState;

	[Export]
	private NavigationAgent3D agent;

	private State currentState;

	private Dictionary<string, Node> states = new();

    public override void _Ready()
    {
		this.InitializeStateMachineAsync();
	}

	private async void InitializeStateMachineAsync()
    {
        await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);

        foreach (Node child in this.GetChildren())
        {
            if (child is State state)
            {
                this.states[state.Name] = state;
                state.Initialize();
            }
        }

        if (this.agent != null)
        {
            this.agent.TargetReached += this.OnTargetReached;
        }

        if (this.defaultState != null)
        {
            this.ChangeState(this.defaultState.Name);
        }
		
		
    }

	public void ChangeState(string stateName)
	{
		if (!this.states.ContainsKey(stateName))
        {
            return;
        }

		State newState = this.states[stateName] as State;

		if (newState == this.currentState)
		{
			return;
		}

		if(this.currentState != null)
		{
			this.currentState.Exit();
		}

		this.currentState = newState;
		newState.Enter();

		GD.Print("StateChanged: " + stateName);
	}

	private void OnTargetReached()
	{
		if(this.currentState != null)
		{
			this.currentState.NavigationComplete();
		}
	}

    public override void _Process(double delta)
    {
        if(this.currentState != null)
		{
			this.currentState.Update(delta);
		}
    }

    public override void _PhysicsProcess(double delta)
    {
		if(this.currentState != null)
		{
			this.currentState.PhysicsUpdate(delta);
		}
    }
}
