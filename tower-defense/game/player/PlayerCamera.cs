using Godot;
using System;

public partial class PlayerCamera : Camera2D
{

    private bool drag;
    [Export]
    private float acceleration = 0.4f;

    public override void _Ready()
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Middle)
        {
            this.drag = mouseButton.Pressed;
        }

        if (@event is InputEventMouseMotion mouseMotion && this.drag)
        {
            this.Position -= mouseMotion.Relative * this.acceleration;
        }
    }
}
