using Godot;
using System;

public partial class Game : Node
{
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}
}
