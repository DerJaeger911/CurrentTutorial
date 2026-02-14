using Adventuregame.Scripts.GlobalData.Preloads;
using Godot;
using System;

public partial class Hud : Control
{
	private HFlowContainer heartsContainer;

	public override void _Ready()
	{
		this.EnsureInitialisation();
	}
	public void Setup(int health)
	{
		this.EnsureInitialisation();
		for (int i = 0; i < health; i++)
		{
			var heartInstance = PreloadScenes.HeartScene.Instantiate<Control>();
			this.heartsContainer.AddChild(heartInstance);
		}
	}

	public void SetHealth(int health)
	{
		this.EnsureInitialisation();
		foreach(Control heart in this.heartsContainer.GetChildren())
		{
			heart.QueueFree();
		}
		this.Setup(health);
	}

	private void EnsureInitialisation()
	{
		if (this.heartsContainer == null)
		{
			this.heartsContainer = this.GetNode<HFlowContainer>("Hearts");
		}
	}
}
