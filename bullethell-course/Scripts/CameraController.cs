using bullethellcourse.Scripts.Entities;
using Godot;
using System;

public partial class CameraController : Camera2D
{
	[Export]
	private float followRate = 2;

	private Node2D target;

    public override void _Ready()
    {
		this.target = this.GetNode<Node2D>("../Player");
        this.GlobalPosition = target.GlobalPosition;
	}

    public override void _Process(Double delta)
    {
            this.GlobalPosition = this.GlobalPosition.Lerp(this.target.GlobalPosition, this.followRate * (float)delta);
    }
}
