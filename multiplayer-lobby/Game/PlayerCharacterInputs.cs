using Godot;
using System;

public partial class PlayerCharacterInputs : MultiplayerSynchronizer
{
	[Export]
	private float moveInput;
	[Export]
	private bool jumpInput;

	public override void _Ready()
	{
		if(this.GetMultiplayerAuthority() != this.Multiplayer.GetUniqueId())
		{
			this.SetPhysicsProcess(false);
		}
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.moveInput = Input.GetAxis("ui_left", "ui_right");
		this.jumpInput = Input.IsActionJustPressed("ui_accept");
	}
}
