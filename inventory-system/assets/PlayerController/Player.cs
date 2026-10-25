using Godot;

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
    public bool IsRunning { get; private set; } = false;

    [ExportGroup("Camera")]
    [Export] private float lookSensitivity = 0.005f;
    private Vector2 cameraLookInput;

    private Camera3D camera;
    private float gravity;

    public override void _Ready()
    {
        // Lock the mouse
        Input.MouseMode = Input.MouseModeEnum.Captured;

        camera = GetNode<Camera3D>("Camera3D");
        gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * gravityModifier;
    }

    public override void _PhysicsProcess(double delta)
    {
        float fDelta = (float)delta;

        // Apply gravity
        Vector3 vel = Velocity;
        if (!IsOnFloor())
        {
            vel.Y -= gravity * fDelta;
        }

        // Jumping
        if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            vel.Y = jumpForce;
        }

        // Movement
        Vector2 moveInput = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        Vector3 moveDir = (Transform.Basis * new Vector3(moveInput.X, 0, moveInput.Y)).Normalized();

        IsRunning = Input.IsActionPressed("sprint");

        float targetSpeed = maxSpeed;

        if (IsRunning)
        {
            targetSpeed = maxRunSpeed;
            float runDot = -moveDir.Dot(Transform.Basis.Z);
            runDot = Mathf.Clamp(runDot, 0.0f, 1.0f);
            moveDir *= runDot;
        }

        float currentSmoothing = acceleration;

        if (!IsOnFloor())
        {
            currentSmoothing = airAcceleration;
        }
        else if (moveDir == Vector3.Zero)
        {
            currentSmoothing = braking;
        }

        Vector3 targetVel = moveDir * targetSpeed;

        vel.X = Mathf.Lerp(vel.X, targetVel.X, currentSmoothing * fDelta);
        vel.Z = Mathf.Lerp(vel.Z, targetVel.Z, currentSmoothing * fDelta);

        Velocity = vel;
        MoveAndSlide();

        // Camera Look
        RotateY(-cameraLookInput.X * lookSensitivity);
        
        Vector3 camRot = camera.Rotation;
        camRot.X -= cameraLookInput.Y * lookSensitivity;
        camRot.X = Mathf.Clamp(camRot.X, -1.5f, 1.5f);
        camera.Rotation = camRot;

        cameraLookInput = Vector2.Zero;

        // Mouse
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
            cameraLookInput = mouseMotion.Relative;
        }
    }
}