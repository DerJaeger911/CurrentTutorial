using Godot;
using Godot3dPlatformerCourse.Scripts.Game.Player;
using System;
using System.Runtime.CompilerServices;

namespace Godot3dPlatformerCourse.Scripts.Game;

public partial class Enemy : Area3D
{
	[Export]
	private float moveSpeed = 2;
	[Export]
	private Vector3 moveDirection;
	[Export]
	private float spinSpeed = 900;

	private Vector3 spawnPosition;
	private Vector3 startPos;
	private Vector3 targetPos;
	private MeshInstance3D model;

	public override void _Ready()
	{
		startPos = GlobalPosition;
		targetPos = GlobalPosition + moveDirection;
		model = GetNode<MeshInstance3D>("Model");

		BodyEntered += OnBodyEntered;

		RandomStartPosition();
	}

	public override void _Process(double delta)
	{
		var modelRotation = model.Rotation;
		modelRotation.Z += ((float)Math.PI / 180f * spinSpeed) * (float)delta;
		model.Rotation = modelRotation;

		GlobalPosition = GlobalPosition.MoveToward(targetPos, moveSpeed * (float)delta);
		if (GlobalPosition == startPos)
		{
			targetPos = startPos + moveDirection;
		} else if (GlobalPosition == startPos + moveDirection)
		{
			targetPos = startPos;
		}
	}

	private void RandomStartPosition()
	{
		float t = (float)GD.RandRange(0.0, 1.0);

		GlobalPosition = startPos.Lerp(targetPos, t);
	}

	public void OnBodyEntered(Node body)
	{
		if (body is IPlayer player)
		{
			player.TakeDamage(1);
		}
	}



	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
	}
}
