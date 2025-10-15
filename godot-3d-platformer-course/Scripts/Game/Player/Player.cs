using Godot;
using Godot3dPlatformerCourse.Scripts.Game.Consts;
using System;

namespace Godot3dPlatformerCourse.Scripts.Game.Player;

public partial class Player : CharacterBody3D, IPlayer
{
	[Export]
	private float moveSpeed = 3;
	[Export]
	private float jumpForce = 8;
	[Export]
	private float gravity = 20;
	[Export]
	private int health = 3;

	private int points;

	[Signal]
	public delegate void OnTakeDamageEventHandler(int hp);
    [Signal]
    public delegate void OnScoreChangedEventHandler(int score);


    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		velocity.Y -= gravity * (float)delta;

		if (Input.IsActionPressed(InputConsts.Jump) && IsOnFloor())
		{
			velocity.Y = jumpForce;
		}

		Vector2 moveInput = Input.GetVector(InputConsts.MoveRight, InputConsts.MoveLeft, InputConsts.MoveBackward, InputConsts.MoveForward);

		Vector3 moveDir = new(moveInput.X, 0,moveInput.Y);

		velocity.X = moveDir.X * moveSpeed;
		velocity.Z = moveDir.Z * moveSpeed;

		Velocity = velocity;

		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		if(GlobalPosition.Y <= -5)
		{
			GameOver();
		}
	}

	public void TakeDamage(int amount)
	{
		health -= amount;
		EmitSignal(SignalName.OnTakeDamage, health);

		if (health <= 0)
		{
			GameOver();
		}
	}

	public void UpdateScore(int amount)
	{
		points += amount;
		PlayerStats.Instance.Score += amount;
		GD.Print(PlayerStats.Instance.Score);
	}

	private void GameOver()
	{
		PlayerStats.Instance.Score = 0;
		EmitSignal(SignalName.OnScoreChanged, PlayerStats.Instance.Score);
	}
}