using Godot;
using Godot.Collections;
using System;

public partial class Ai : CharacterBody3D
{

[Export]
private float walkSpeed = 1.0f;

[Export]
private float runSpeed = 2.5f;

[Export]
private NavigationAgent3D agent;

public bool IsRunning {get; set;}
public bool IsStopped {get; set;}
public bool LookAtPlayer {get; set;} = false;

private Vector3 moveDirection;
private float targetYRot;
private Vector3 velocity;

private float gravity;

public Player PlayerCharacter {get; set;}
public float PlayerDistance {get; set;}

	public override void _Ready()
	{
		this.gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
		this.PlayerCharacter = (Player)this.GetTree().GetFirstNodeInGroup("Player");

		this.velocity = this.Velocity;
	}

	public override void _Process(double delta)
	{
		if (this.PlayerCharacter != null)
		{
			this.PlayerDistance = this.Position.DistanceTo(this.PlayerCharacter.Position);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		float fDelta = (float)delta;

		if (!this.IsOnFloor())
		{
			this.velocity.Y -= this.gravity * fDelta;
		}
		else
		{
			this.velocity.Y = 0f;
		}

		Vector3 targetPosition = this.agent.GetNextPathPosition();
		Vector3 moveDir = this.Position.DirectionTo(targetPosition);

		moveDir.Y = 0;
		moveDir = moveDir.Normalized();

		if(this.agent.IsNavigationFinished() || this.IsStopped)
		{
			moveDir = Vector3.Zero;
		}

		float currentSpeed = this.walkSpeed;

		if (this.IsRunning)
		{
			currentSpeed = this.runSpeed;
		} 

		this.velocity.X = moveDir.X * currentSpeed;
		this.velocity.Z = moveDir.Z * currentSpeed;

		this.Velocity = this.velocity; 

		MoveAndSlide();

		// Rotation nur hier einmalig berechnen (mit + Mathf.Pi für die exakte Ausrichtung)
		if (this.LookAtPlayer && this.PlayerCharacter != null)
		{
			Vector3 playerDir = this.PlayerCharacter.Position - this.Position;
			this.targetYRot = Mathf.Atan2(playerDir.X, playerDir.Z) + Mathf.Pi;
		}
		else if (new Vector2(this.velocity.X, this.velocity.Z).Length() > 0.1f)
		{
			this.targetYRot = Mathf.Atan2(this.velocity.X, this.velocity.Z) + Mathf.Pi;
		}

		Vector3 currentRot = this.Rotation;
		currentRot.Y = Mathf.LerpAngle(currentRot.Y, this.targetYRot - 3.14f, 0.1f);
		this.Rotation = currentRot;
	}

	public void MoveToPosition(Vector3 toPosition, bool adjustPosition = true)
	{
		this.IsStopped = false;

		if (adjustPosition)
		{
			Rid map = this.GetWorld3D().NavigationMap;
			var adjustedPosition = NavigationServer3D.MapGetClosestPoint(map, toPosition);
			this.agent.TargetPosition = adjustedPosition;
		}
		else
		{
			this.agent.TargetPosition = toPosition;
		}
	}
}
