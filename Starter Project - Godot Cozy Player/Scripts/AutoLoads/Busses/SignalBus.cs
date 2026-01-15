using Godot;

namespace Dcozysandbox.Scripts.AutoLoads.Busses;

internal partial class SignalBus : GodotObject
{
	public static readonly SignalBus Instance = new();
	private SignalBus() 
	{
		GD.Print("SignalBus is loaded");
	}

	[Signal]
	public delegate void ToolChangedEventHandler( int index);

	[Signal]
	public delegate void ToolInteractEventHandler(int tool, Vector2 position); 
}
