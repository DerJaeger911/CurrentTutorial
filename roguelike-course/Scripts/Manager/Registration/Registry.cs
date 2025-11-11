using Godot;
using RoguelikeCourse.Scripts.Manager;

public partial class Registry : Node
{
	public override void _Ready()
	{
		this.AddChild(new SingletonRegistry());
		this.AddChild(new RootTreeAdd());
		GD.Print("HI");
	}
}
