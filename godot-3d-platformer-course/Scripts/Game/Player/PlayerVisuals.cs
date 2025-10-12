using Godot;
using System;

namespace Godot3dPlatformerCourse.Scripts.Game.Player;

public partial class PlayerVisuals : MeshInstance3D
{
	[Export]
	float rotateRate = 20;
	
	float targetYRot = 0;

	private CharacterBody3D player;

	public override void _Ready()
	{
		player = GetParent<CharacterBody3D>();
	}

	public override void _Process(double delta)
	{
		SmoothRotation(delta);
		MoveBob(delta);
	}

	private void SmoothRotation(double delta)
	{
		var velocity = player.Velocity;
		var rotation = Rotation;

		if (velocity.X != 0 || velocity.Z != 0)
		{
			targetYRot = MathF.Atan2(velocity.X, velocity.Z);
		}
		rotation.Y = Mathf.LerpAngle(Rotation.Y, targetYRot, rotateRate * (float)delta);
		Rotation = rotation;
	}

	private void MoveBob(double delta)
	{
		float moveSpeed = player.Velocity.Length();
		Vector3 scale = Scale;

		if ( moveSpeed < 0.1 || !player.IsOnFloor())
		{
			scale.Y = 1;
		}
		else
		{
			float time = (float)Time.GetTicksMsec() / 100;
			scale.Y = 1 + (MathF.Sin((float)time) * 0.08f);
		}
		Scale = scale;
	}
}
