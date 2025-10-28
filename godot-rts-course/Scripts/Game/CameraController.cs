using Godot;
using System;

namespace GodotRTSCourse.Scripts.Game;

public partial class CameraController : Camera2D
{
    [Export]
    private float moveSpeed = 70;
    [Export]
    private float zoomAmount = 0.2f;

    private readonly float clampMax = 5; 

    public override void _Process(double delta)
    {
        this.CameraMove(delta);
        this.CameraZoom(delta);
    }

    private void CameraMove(double delta)
    {
        Vector2 input = Input.GetVector("cam_left", "cam_right", "cam_up", "cam_down");

        float zoomMod = (this.clampMax + 1) - this.Zoom.X;

        this.GlobalPosition += input * (float)delta * this.moveSpeed  * zoomMod;
    }

    private void CameraZoom(double delta)
    {
        float z = this.Zoom.X;

        if (Input.IsActionJustPressed("zoom_in"))
        {
            z += this.zoomAmount;
        }
        else if (Input.IsActionJustPressed("zoom_out"))
        {
            z -= this.zoomAmount;
        }

        z = Math.Clamp(z, 1, this.clampMax);

        this.Zoom = new Vector2(z, z);
    }
}
