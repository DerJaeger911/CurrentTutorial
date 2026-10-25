using Godot;
using System;

public partial class ChaseState : State
{
	[Export]
	private float fleeRange = 2f;
	[Export]
	private float loseInterestRange = 5f;

	private float pathUpdateRate = 0.1f;
	private double lastPathUpdateTime;

	public override void Enter()
	{
		base.Enter();
		this.Controller.IsRunning = true;

		// Direkt beim Betreten die aktuelle Zeit setzen und sofort den Pfad holen!
		this.lastPathUpdateTime = Time.GetUnixTimeFromSystem();
		UpdatePath();
	}

	public override void Exit()
	{
		base.Exit();
		this.Controller.IsRunning = false;
	}

	public override void Update(double delta)
	{
		// 1. Zuerst prüfen, ob wir flüchten müssen
		if (this.Controller.PlayerDistance < this.fleeRange)
		{
			this.StateMachine.ChangeState("Flee");
			return; // WICHTIG: Hier sofort rausspringen, damit nichts mehr überschrieben wird!
		}

		// 2. Dann prüfen, ob wir das Interesse verlieren
		if (this.Controller.PlayerDistance > this.loseInterestRange)
		{
			this.StateMachine.ChangeState("Wander");
			return; // WICHTIG: Auch hier sofort rausspringen!
		}

		// 3. Erst wenn KEIN State-Wechsel stattfindet, normal weiterverfolgen
		this.Controller.IsStopped = false;

		double currentTime = Time.GetUnixTimeFromSystem();

		if (currentTime - this.lastPathUpdateTime > this.pathUpdateRate)
		{
			this.lastPathUpdateTime = currentTime;
			UpdatePath();
		}
	}

	private void UpdatePath()
	{
		if (this.Controller?.PlayerCharacter != null)
		{
			this.Controller.MoveToPosition(this.Controller.PlayerCharacter.Position, false);
		}
	}
}