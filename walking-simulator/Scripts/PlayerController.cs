using Godot;
using System;

namespace WalkingSimulator.Scripts;

public partial class PlayerController : CharacterBody3D
{
	[ExportGroup("MoveSettings")]
	[Export]
	private float maxSpeed = 4;
	[ExportGroup("MoveSettings")]
	[Export]
	private float acceleration = 20;
	[ExportGroup("MoveSettings")]
	[Export]
	private float braking = 20;
	[ExportGroup("MoveSettings")]
	[Export]
	private float airAcceleration = 4;
	[ExportGroup("MoveSettings")]
	[Export]
	private float jumpForce = 5;
	[ExportGroup("MoveSettings")]
	[Export]
	private float gravityModifier = 1.5f;
	private float gravity;
	[ExportGroup("MoveSettings")]
	[Export]
	private float maxRunSpeed = 6;
	private bool isRunning = false;

	[ExportGroup("Camera")]
	[Export]
	private float lookSensitivity = 0.005f;
	private Vector2 cameraLookInput;
	private Camera3D camera;

	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera3D");
		gravity = (float)ProjectSettings.GetSetting("physics/3D/default_gravity") * gravityModifier;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		var moveInput = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Velocity = new Vector3(moveInput.X, 0, moveInput.Y) * maxSpeed;
		MoveAndSlide();

		this.RotateY(-cameraLookInput.X * lookSensitivity);

		camera.RotateX(-cameraLookInput.Y * lookSensitivity);
		var cameraRotation = camera.Rotation;
		cameraRotation.X = Math.Clamp(camera.Rotation.X, -1.5f, 1.5f);
		camera.Rotation = new Vector3(cameraRotation.X, cameraRotation.Y, cameraRotation.Z);

		cameraLookInput = Vector2.Zero;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseMotion mouseMotion)
		{
			cameraLookInput = mouseMotion.Relative;
		}
	}
}
