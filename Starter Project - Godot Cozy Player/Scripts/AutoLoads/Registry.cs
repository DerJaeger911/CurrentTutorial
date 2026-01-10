using Dcozysandbox.Scripts.AutoLoads.Busses;
using Godot;
using System;

internal partial class Registry : Node
{
	public override void _Ready()
	{
		_ = SignalBus.Instance;
	}
}
