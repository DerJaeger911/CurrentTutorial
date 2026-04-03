using Godot;
using System;

public partial class SignalHub : Node
{
	public static SignalHub Instance { get; private set; }

	override public void _Ready()
	{
		if (Instance != null)
		{
			GD.PrintErr("Multiple instances of SignalHub detected. This is not allowed.");
			this.QueueFree();
			return;
		}
		Instance = this;
	}

	[Signal]
	public delegate void ShootEventHandler(Vector2 position, Vector2 direction);

	public void EmitShoot(Vector2 position, Vector2 direction)
	{
		this.EmitSignal(SignalName.Shoot, position, direction);
	}
}
