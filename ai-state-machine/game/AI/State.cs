using Godot;
using System;

public partial class State : Node
{
	[Export]
	public StateMachine StateMachine {get; set;}
	
	[Export]
	protected Ai Controller {get; set;}

	protected bool Active {get; set;}


	public override void _Ready()
	{

	}

	public void Initialize()
	{
		
	}

	public virtual void Enter()
	{
		this.Active = true;
	}

	public virtual void Exit()
	{
		this.Active = false;
	}

	public virtual void Update(double delta)
	{

	}

	public virtual void PhysicsUpdate(double delta)
	{

	}

	public async virtual void NavigationComplete()
	{

	}

	public Vector3 RandomOffset()
	{
		Vector3 offset = new Vector3(GD.RandRange(-1,1),0,GD.RandRange(-1,1));

		return offset.Normalized();
	}
}
