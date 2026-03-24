using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export] public float Speed = 50;
	[Export] public float JumpVelocity = -150;
	[Export] public float Gravity = 400;

	private MultiplayerSynchronizer synchronizer;

	public override void _EnterTree()
	{
		this.GetNode<MultiplayerSynchronizer>("InputSync").SetMultiplayerAuthority(int.Parse(this.Name));
	}

	public override void _Ready()
	{
		this.synchronizer = this.GetNode<MultiplayerSynchronizer>("InputSync");
	}

	public override void _PhysicsProcess(Double delta)
	{
		if (this.Multiplayer.IsServer())
		{
			this.Move(delta);
		}
	}

	public void Move(double delta)
	{
		Vector2 velocity = this.Velocity;

		if (!this.IsOnFloor())
		{
			velocity.Y += this.Gravity * (float)delta;
		}

		if (Input.IsActionJustPressed("ui_accept") && this.IsOnFloor())
		{
			velocity.Y = this.JumpVelocity;
		}

		float direction = Input.GetAxis("ui_left", "ui_right");

		if (direction != 0)
		{
			velocity.X = direction * this.Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(this.Velocity.X, 0, this.Speed);
		}

		this.Velocity = velocity;
		this.MoveAndSlide();
	}
}
