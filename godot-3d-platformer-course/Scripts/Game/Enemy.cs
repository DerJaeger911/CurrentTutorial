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

	public override void _Ready()
	{
		Vector3 startPos = GlobalPosition;
		Vector3 targetPos = GlobalPosition + moveDirection;
		MeshInstance3D model = GetNode<MeshInstance3D>("Model");

		BodyEntered += OnBodyEntered;

		RandomStartPosition();
	}

	public override void _Process(double delta)
	{
		
	}

	private void RandomStartPosition()
	{

	}

	public void OnBodyEntered(Node body)
	{
		if (body is IPlayer player)
		{
			GD.Print("Oh, fuck me daddy");
		}
	}

	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
	}
}
