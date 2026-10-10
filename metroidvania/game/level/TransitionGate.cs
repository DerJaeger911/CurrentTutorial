using Godot;
using System;

public partial class TransitionGate : Area2D
{
	[Export]
	private LevelEnums nextLevel;
	private TransitionLayer transitionLayer;

	public override void _Ready()
	{
		BodyEntered += this.OnBodyEntered;
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.Freeze();
			TransitionLayer.Instance.Transition(this.nextLevel, Data.Instance.CurrentLevel);
		}
	}
}
