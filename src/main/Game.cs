using Godot;
using System;
using twentyfourtyeight.src.main.Mangager;

public partial class Game : Node
{
	public override void _Ready()
	{
		AssetManager manager = new AssetManager();
		this.AddChild(manager);
	}
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}

	public override void _ExitTree()
	{
		AssetManager.Instance.Unload();
	}
}
