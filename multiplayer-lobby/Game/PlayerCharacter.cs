using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export] public float Speed = 50;
	//[Export] public float JumpVelocity = -150;
	//[Export] public float Gravity = 400;

	private PlayerCharacterInputs inputSync;

	public override void _EnterTree()
	{
		int id = int.Parse(this.Name);
		this.SetMultiplayerAuthority(1);
		this.GetNode<PlayerCharacterInputs>("InputSync").SetMultiplayerAuthority(id);
		this.GetNode<MultiplayerSynchronizer>("CharacterSync").SetMultiplayerAuthority(1);
	}

	public override void _Ready()
	{
		this.inputSync = this.GetNode<PlayerCharacterInputs>("InputSync");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (this.Multiplayer.IsServer())
		{
			this.Move(delta);
		}
		GD.Print(this.Position);
	}

	public void Move(double delta)
	{
		Vector2 velocity = this.Velocity;

		if (!this.IsOnFloor())
		{
			//velocity.Y += this.Gravity * (float)delta;
		}

		if (this.inputSync.JumpInput && this.IsOnFloor())
		{
			//velocity.Y = this.JumpVelocity;
		}

		float direction = this.inputSync.MoveInput;

		if (!Mathf.IsZeroApprox(direction))
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
