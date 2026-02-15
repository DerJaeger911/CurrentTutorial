using Godot;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Adventuregame.Scripts.GlobalData;

internal partial class PauseLogic : Node
{
	public static PauseLogic Instance { get; private set; }

	private Inventory inventory;
	private Timer pauseActivationTimer;

	override public void _Ready()
	{
		Instance = this;
		this.inventory = (Inventory)this.GetTree().GetFirstNodeInGroup("INVENTORY");

		this.pauseActivationTimer = new Timer();
		this.pauseActivationTimer.OneShot = true;
		this.pauseActivationTimer.WaitTime = 0.1f;
		this.pauseActivationTimer.ProcessMode = ProcessModeEnum.Always;
		this.AddChild(this.pauseActivationTimer);
	}

	public void TogglePause(bool value)
	{
		if (this.pauseActivationTimer.IsStopped())
		{
			this.GetTree().Paused = value;
			this.pauseActivationTimer.Start();
			this.ShowInventory(value);
		}
	}

	private void ShowInventory(bool value)
	{
		if (value) 
		{
			this.inventory.Reveal();
		}
		else
		{
			this.inventory.Hide();
		}
	}
}
