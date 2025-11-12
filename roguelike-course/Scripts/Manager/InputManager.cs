using Godot;

namespace RoguelikeCourse.Scripts.Manager;

public partial class InputManager : Node
{
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}
}