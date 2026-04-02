using Godot;

namespace MicroTrunbasedRPG.Game;

public partial class SignalHub:Node
{
	public static SignalHub Instance { get; private set; }

	public override void _EnterTree()
	{
		Instance = this;
	}

	[Signal]
	public delegate void GameLoserEventHandler(Character character);

	public void EmitGameLoser(Character character)
	{
		this.EmitSignal(SignalName.GameLoser, character);
	}
}
