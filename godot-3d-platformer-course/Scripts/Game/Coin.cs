using Godot;
using Godot3dPlatformerCourse.Scripts.Game.Player;
using System;

public partial class Coin : Area3D
{
	[Export]
	private float bobHeight = 0.2f;
	[Export] 
	private float bobSpeed = 3;
	[Export]
	float rotateSpeed = 300;

	private Vector3 startPos;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        startPos = GlobalPosition;
    }

    public override void _Process(double delta)
	{
        float time = (float)Time.GetTicksMsec() / 1000;
		
        float posY = (1 + MathF.Sin((float)time * bobSpeed)) / 2 * bobHeight;

        GlobalPosition = new Vector3(startPos.X, startPos.Y + posY, startPos.Z);

        Vector3 rotation = Rotation;
		rotation.Y += ((float)Math.PI /180 * rotateSpeed) * (float)delta;
		Rotation = rotation;
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
