using Godot;
using System;
using System.Runtime.CompilerServices;

namespace Adventuregame.Scripts.Characters.Player;

public partial class Player : Character
{
    [Export]
    private float acceleration = 8;
    [Export]
    private float deceleraation = 4;

    private Camera3D camera;
    private Node3D playerSkin;

    override public void _Ready()
    {
        base._Ready();
        this.camera = this.GetNode<Camera3D>("CameraController/Camera3D");
        this.playerSkin = this.GetNode<Node3D>("PlayerSkin");
	}

	public override void _PhysicsProcess(Double delta)
    {
        this.MoveLogic(delta);
        this.MoveAndSlide();
    }

    private void MoveLogic(Double delta)
    {
        this.MovementInput = Input.GetVector("left", "right", "forward", "backward").Rotated(-this.camera.GlobalRotation.Y);
        Vector2 velocity2d = new Vector2(this.Velocity.X, this.Velocity.Z);

        if(this.MovementInput != Vector2.Zero)
        {
            velocity2d += this.MovementInput * this.BaseSpeed * (float)delta * this.acceleration;
            velocity2d = velocity2d.LimitLength(this.BaseSpeed);
            var targetAngle = -this.MovementInput.Angle() + float.Pi / 2;
            double playerYRotation = Mathf.RotateToward(this.playerSkin.Rotation.Y, targetAngle, delta * 6);
			this.playerSkin.Rotation = new Vector3(this.playerSkin.Rotation.X, (float)playerYRotation, this.playerSkin.Rotation.Z);
		}
        else
        {
            velocity2d = velocity2d.MoveToward(Vector2.Zero, this.BaseSpeed * (float)delta * this.deceleraation);
		}
		this.Velocity = new Vector3(velocity2d.X, this.Velocity.Y, velocity2d.Y);
	}
}
