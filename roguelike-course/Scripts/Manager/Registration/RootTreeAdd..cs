using Godot;

namespace RoguelikeCourse.Scripts.Manager;

internal partial class RootTreeAdd : Node
{
	public override void _Ready()
	{
		this.AddChild(new InputManager());
	}
}
