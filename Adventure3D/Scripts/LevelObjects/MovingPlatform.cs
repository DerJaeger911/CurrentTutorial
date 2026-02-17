using Godot;
using System;

public partial class MovingPlatform : AnimatableBody3D
{
	[Export]
	private float speed = 4;

	private int direction = 1;
	private Area3D area;

	public override void _Ready()
	{
		this.area = this.GetNode<Area3D>("Area3D");
		this.area.BodyEntered += this.OnBodyEntered;
	}

	override public void _PhysicsProcess(double delta)
	{
		this.Position += (new Vector3(this.speed * this.direction * (float)delta, 0, 0));	
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body != this)
		{
			this.direction *= -1;
		}
	}

	public override void _ExitTree()
	{
		this.area.BodyEntered -= this.OnBodyEntered;
	}
}
