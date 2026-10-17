using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [ExportGroup("Slots")]
    [Export]
    private Camera3D camera;
    [Export]
    private Node3D head;

    [ExportGroup("Movement")]
    [Export]
    private float moveSpeed = 5f;
    [Export]
    private float jumpForce = 5f;
    [Export]
    private float gravity = 9f;

    [ExportGroup("Camera")]
    [Export]
    private float lookSens = 5f;
    [Export]
    private float minXRot = -85f;
    [Export]
    private float maxXRot = 85f;

    private Vector2 mouseDir;

    private float rotX = 0f;
    private float rotY = 0f;

    public override void _Ready()
    {
        this.RemoveChild(this.camera);
        this.GetNode("/root/Main").CallDeferred(DeferredConsts.AddChild, this.camera);
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            this.rotX += mouseMotion.Relative.Y * -this.lookSens;

            this.rotX = Mathf.Clamp(this.rotX, this.minXRot, this.maxXRot);

            this.rotY += mouseMotion.Relative.X * -this.lookSens;

            this.camera.RotationDegrees = new Vector3( 
                this.rotX,
                rotY,
                this.camera.RotationDegrees.Z);
        }
    }

    public override void _Process(double delta)
    {
        this.camera.GlobalPosition = this.head.GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
    }

}
