using Godot;
using Godot3dPlatformerCourse.Scripts.Game.Player;

public partial class Coin : Area3D
{
	[Export]
	float rotateSpeed = 180;

	public override void _Process(double delta)
	{
		
	}

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public void OnBodyEntered(Node body)
	{
		if (body is IPlayer player)
		{
			player.UpdateScore(1);
			QueueFree();
		}
	}

	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
	}
}
