using Godot;

namespace RoguelikeCourse.Scripts.Manager;

public partial class InputManager : Node
{
	private int c;

	public override void _Process(System.Double delta)
	{
		GD.Print(this.c++);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}
}