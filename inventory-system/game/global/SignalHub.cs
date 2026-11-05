using Godot;
using System;

public partial class SignalHub : Node
{
	public static SignalHub Instance { get; private set; }

	public override void _Ready()
	{
		// Setzt die Instanz beim Start der Szene
		Instance = this;
	}

	[Signal]
	public delegate void GivePlayerItemEventHandler(Item item, int amount);

	public void EmitGivePlayerItem(Item item, int amount)
    {
        this.EmitSignal(SignalName.GivePlayerItem, item, amount);
    }
}
