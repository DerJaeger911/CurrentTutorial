using bullethellcourse.Scripts.Entities;
using Godot;
using System;

namespace bullethellcourse.Scripts;

public partial class CameraController : Camera2D
{
	[Export]
	private float followRate = 2;
	[Export]
	private float cameraShakeIntensity;



	private Node2D target;

	public override void _Ready()
	{
		this.target = this.GetNode<Node2D>("../Player");
		this.GlobalPosition = this.target.GlobalPosition;
	}

	public override void _Process(Double delta)
	{
		this.GlobalPosition = this.GlobalPosition.Lerp(this.target.GlobalPosition, this.followRate * (float)delta);
		if(this.cameraShakeIntensity > 0)
		{
			this.cameraShakeIntensity = Mathf.Lerp(this.cameraShakeIntensity, 0, (float)delta * 10);
			this.Offset = this.GetRandomOffset();
		}
	}

	public void DamageShake()
	{
		this.cameraShakeIntensity = 8;
	}

	private Vector2 GetRandomOffset()
	{
		float x = (float)GD.RandRange(-this.cameraShakeIntensity, this.cameraShakeIntensity);
		float y = (float)GD.RandRange(-this.cameraShakeIntensity, this.cameraShakeIntensity);
		return new Vector2(x, y);
	}

}
