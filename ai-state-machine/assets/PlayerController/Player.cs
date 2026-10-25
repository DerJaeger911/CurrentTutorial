using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [ExportGroup("Movement")]
    [Export] private float maxSpeed = 4.0f;
    [Export] private float acceleration = 20.0f;
    [Export] private float braking = 20.0f;
    [Export] private float airAcceleration = 4.0f;
    [Export] private float jumpForce = 5.0f;
    [Export] private float gravityModifier = 1.5f;
    [Export] private float maxRunSpeed = 6.0f;
    private bool isRunning = false;

    [ExportGroup("Camera")]
    [Export] private float lookSensitivity = 0.005f;
    private Vector2 cameraLookInput = Vector2.Zero;

    private Camera3D camera;
    private float gravity;

    public override void _Ready()
    {
        this.camera = GetNode<Camera3D>("Camera3D");
        this.gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * this.gravityModifier;
        
        // Lock the mouse
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        float fDelta = (float)delta;

        // Apply gravity
        if (!IsOnFloor())
        {
            Vector3 vel = this.Velocity;
            vel.Y -= this.gravity * fDelta;
            this.Velocity = vel;
        }

        // Jumping
        if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            Vector3 vel = this.Velocity;
            vel.Y = this.jumpForce;
            this.Velocity = vel;
        }

        // Movement
        Vector2 moveInput = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        Vector3 moveDir = (this.Transform.Basis * new Vector3(moveInput.X, 0, moveInput.Y)).Normalized();

        this.isRunning = Input.IsActionPressed("sprint");

        float targetSpeed = this.maxSpeed;

        if (this.isRunning)
        {
            targetSpeed = this.maxRunSpeed;
            float runDot = -moveDir.Dot(this.Transform.Basis.Z);
            runDot = Mathf.Clamp(runDot, 0.0f, 1.0f);
            moveDir *= runDot;
        }

        float currentSmoothing = this.acceleration;

        if (!IsOnFloor())
        {
            currentSmoothing = this.airAcceleration;
        }
        else if (moveDir == Vector3.Zero)
        {
            currentSmoothing = this.braking;
        }

        Vector3 targetVel = moveDir * targetSpeed;
        Vector3 currentVel = this.Velocity;

        currentVel.X = Mathf.Lerp(currentVel.X, targetVel.X, currentSmoothing * fDelta);
        currentVel.Z = Mathf.Lerp(currentVel.Z, targetVel.Z, currentSmoothing * fDelta);
        this.Velocity = currentVel;

        MoveAndSlide();

        // Camera Look
        RotateY(-this.cameraLookInput.X * this.lookSensitivity);
        
        if (this.camera != null)
        {
            this.camera.RotateX(-this.cameraLookInput.Y * this.lookSensitivity);
            Vector3 camRot = this.camera.Rotation;
            camRot.X = Mathf.Clamp(camRot.X, -1.5f, 1.5f);
            this.camera.Rotation = camRot;
        }

        this.cameraLookInput = Vector2.Zero;

        // Mouse Toggle via Escape / ui_cancel
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Visible)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            else
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            this.cameraLookInput = mouseMotion.Relative;
        }
    }
}