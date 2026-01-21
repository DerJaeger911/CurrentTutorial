using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.AutoLoads.Managers;
using Godot;
using System;

internal partial class Registry : Node
{
	public override void _Ready()
	{
		_ = SignalBus.Instance;
		_ = ScenePreloadManager.Instance;
		_ = PlayerResourceManager.Instance;
		PlayerResourceManager.Instance.PrintAll();
	}
}
