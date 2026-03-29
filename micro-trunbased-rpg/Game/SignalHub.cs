using Godot;

namespace MicroTrunbasedRPG.Game;

public partial class SignalHub:Node
{
	public static SignalHub Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	[Signal]
	public delegate void HealEventHandler(int health);

	[Signal]
	public delegate void TakeDamageEventHandler(int health);


	public static void EmitHeal(int health) =>
		Instance.EmitSignal(SignalName.Heal, health);

	public static void EmitTakeDamage(int health) =>
		Instance.EmitSignal(SignalName.TakeDamage, health);
}
