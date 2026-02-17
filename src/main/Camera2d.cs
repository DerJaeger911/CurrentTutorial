using Godot;
using System;

public partial class Camera2d : Camera2D
{
	[Export]
	private int cameravelocity = 15;

	[Export]
	private float zoomSpeed = 0.05f;

	public override void _PhysicsProcess(Double delta)
	{

	}
}
