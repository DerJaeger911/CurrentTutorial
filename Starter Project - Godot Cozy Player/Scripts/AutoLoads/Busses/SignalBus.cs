using Dcozysandbox.Scripts.Enums;
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

	[Signal]
	public delegate void ToolAnimationFinishedEventHandler();

	[Signal]
	public delegate void CanFishEventHandler(bool canFish);

	[Signal]
	public delegate void SeedChangedEventHandler(int seed);

	[Signal]
	public delegate void SeedInteractEventHandler(int seed, Vector2 position);
}
