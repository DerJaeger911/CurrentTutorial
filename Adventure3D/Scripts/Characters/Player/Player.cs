using Godot;
using System;
using Adventuregame.Scripts.GlobalData;
using Adventuregame.Scripts.GlobalData.ObjectDataClasses;

namespace Adventuregame.Scripts.Characters.Player;

public partial class Player : Character
{
    [Export]
    private float acceleration = 8;
    [Export]
    private float deceleraation = 4;



    private Camera3D camera;
    private Node3D playerSkin;
    private WeaponData currentWeapon;
    private int weaponIndex;
    private BoneAttachment3D rightHand;

	override public void _Ready()
    {
        base._Ready();
		this.playerSkin = this.GetNode<Node3D>("PlayerSkin");
		this.rightHand = this.playerSkin.GetNode<BoneAttachment3D>("Rogue/Rig/Skeleton3D/RightHand");
        this.currentWeapon = Global.Instance.Weapons["staff"];
        this.camera = this.GetNode<Camera3D>("CameraController/Camera3D");
		this.Equip(this.currentWeapon, this.rightHand);
	}

	public override void _PhysicsProcess(Double delta)
    {
        this.MoveLogic(delta);
        this.JumpLogic(delta);
        this.MoveAndSlide();
    }

    private void MoveLogic(Double delta)
    {
        this.MovementInput = Input.GetVector("left", "right", "forward", "backward").Rotated(-this.camera.GlobalRotation.Y);
        Vector2 velocity2d = new Vector2(this.Velocity.X, this.Velocity.Z);

        if (this.MovementInput != Vector2.Zero)
        {
            velocity2d += this.MovementInput * this.BaseSpeed * (float)delta * this.acceleration;
            velocity2d = velocity2d.LimitLength(this.BaseSpeed);
            var targetAngle = -this.MovementInput.Angle() + float.Pi / 2;
            double playerYRotation = Mathf.RotateToward(this.playerSkin.Rotation.Y, targetAngle, delta * 6);
            this.playerSkin.Rotation = new Vector3(this.playerSkin.Rotation.X, (float)playerYRotation, this.playerSkin.Rotation.Z);
			if (this.IsOnFloor())
			{
				this.SetMoveState("Running_A");
			} 
		}
        else
        {
            velocity2d = velocity2d.MoveToward(Vector2.Zero, this.BaseSpeed * (float)delta * this.deceleraation);
			if (this.IsOnFloor())
			{
				this.SetMoveState("Idle");
			}
		}
        this.Velocity = new Vector3(velocity2d.X, this.Velocity.Y, velocity2d.Y);
	}

    private void JumpLogic(double delta)
    {
        if (this.IsOnFloor())
        {
            if (Input.IsActionJustPressed("jump"))
            {
                this.Velocity = new Vector3(this.Velocity.X, -this.JumpVelocity, this.Velocity.Z);
            }
        }
        else 
        {
            this.SetMoveState("Jump_Idle");
        }
		var gravity = this.JumpVelocity < 0 ? this.JumpGravity : this.FallGravity;
        this.ApplyGravity(gravity, delta);
	}

    private void ApplyGravity(float gravity, double delta)
    {
        this.Velocity = new Vector3(this.Velocity.X, this.Velocity.Y - gravity * (float)delta, this.Velocity.Z);
	}


}
