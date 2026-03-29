using Godot;
using System;

public partial class PlayerCharacterInputs : MultiplayerSynchronizer
{
	[Export] public float MoveInput;
	[Export] public bool JumpInput;

	public override void _Ready()
	{
		if (this.GetMultiplayerAuthority() != this.Multiplayer.GetUniqueId())
		{
			this.SetProcess(false);
			this.SetPhysicsProcess(false);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		this.MoveInput = Input.GetAxis("ui_left", "ui_right");

		this.JumpInput = Input.IsActionPressed("ui_accept");
	}
}