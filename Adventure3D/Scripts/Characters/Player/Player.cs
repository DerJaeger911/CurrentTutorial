using Godot;
using System;
using Adventuregame.Scripts.GlobalData;
using Adventuregame.Scripts.GlobalData.ObjectDataClasses;
using Adventuregame.Scripts.GlobalData.Enums;
using System.Collections.Generic;

namespace Adventuregame.Scripts.Characters.Player;

public partial class Player : Character
{
	[Export]
	private float acceleration = 8;
	[Export]
	private float deceleraation = 4;



	private Camera3D camera;
	private Node3D playerSkin;
	private WeaponData currentWeaponData;
	private ShieldData currentShieldData;
	private StyleData currentStyleData;
	private WeaponEnum currentWeaponIndex = WeaponEnum.Dagger;
	private ShieldEnum currentShieldIndex = ShieldEnum.Square;
	private StyleEnum currentStyleIndex = StyleEnum.Duckhat;
	private BoneAttachment3D rightHand;
	private BoneAttachment3D leftHand;
	private BoneAttachment3D head;

    override public void _Ready()
	{
		base._Ready();
		this.playerSkin = this.GetNode<Node3D>("PlayerSkin");
		this.rightHand = this.playerSkin.GetNode<BoneAttachment3D>("Rogue/Rig/Skeleton3D/RightHand");
		this.leftHand = this.playerSkin.GetNode<BoneAttachment3D>("Rogue/Rig/Skeleton3D/LeftHand");
		this.head = this.playerSkin.GetNode<BoneAttachment3D>("Rogue/Rig/Skeleton3D/Head");
		this.currentWeaponData = Global.Instance.Weapons[this.currentWeaponIndex];
		this.currentShieldData = Global.Instance.Shields[this.currentShieldIndex];
		this.currentStyleData = Global.Instance.Style[this.currentStyleIndex];
		this.camera = this.GetNode<Camera3D>("CameraController/Camera3D");
		this.Equip(this.currentWeaponData, this.rightHand);
		this.Equip(this.currentShieldData, this.leftHand);
		this.Equip(this.currentStyleData, this.head);
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.MoveLogic(delta);
		this.JumpLogic(delta);
		this.AbbilityLogic();
		this.MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (!this.Attacking)
		{
			if (Input.IsActionJustPressed("switch_weapon"))
			{
				this.SwitchItem(ref this.currentWeaponIndex, this.AllWeapons, Global.Instance.Weapons, this.rightHand);
			}

			if (Input.IsActionJustPressed("switch_shield"))
			{
				this.SwitchItem(ref this.currentShieldIndex, this.AllShields, Global.Instance.Shields, this.leftHand);
			}
		}
	}

	private void SwitchItem<TEnum, TData>(ref TEnum currentIndex, TEnum[] allValues, Dictionary<TEnum, TData> dataSource, BoneAttachment3D slot)
		where TEnum : struct, Enum
		where TData : ItemDataBase
	{
		int nextIntIndex = ((int)(object)currentIndex + 1) % allValues.Length;
		currentIndex = allValues[nextIntIndex];

		if (dataSource.TryGetValue(currentIndex, out var data))
		{
			this.Equip(data, slot);
		}
	}

	private void MoveLogic(Double delta)
	{
		this.MovementInput = Input.GetVector("left", "right", "forward", "backward").Rotated(-this.camera.GlobalRotation.Y);
		Vector2 velocity2d = new Vector2(this.Velocity.X, this.Velocity.Z);

		if (this.MovementInput != Vector2.Zero)
		{
			var speed = Input.IsActionPressed("run") ? this.RunSpeed : this.BaseSpeed;
			speed = this.Defending ? this.DefendSpeed : speed;
			velocity2d += this.MovementInput * speed * (float)delta * this.acceleration;
			velocity2d = velocity2d.LimitLength(speed);
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
			this.JumpCount = 0;
		}
		else
		{
			this.SetMoveState("Jump_Idle");
		}
		
		if (this.JumpCount <= this.MaxJumpCount)
		{
			if (Input.IsActionJustPressed("jump"))
			{
				this.Velocity = new Vector3(this.Velocity.X, -this.JumpVelocity, this.Velocity.Z);
				this.JumpCount++;
			}
		}

		var gravity = this.JumpVelocity < 0 ? this.JumpGravity : this.FallGravity;
		this.ApplyGravity(gravity, delta);
	}

	private void AbbilityLogic()
	{
		this.Defending = Input.IsActionPressed("defend");
		if (Input.IsActionJustPressed("attack") && !this.Attacking)
		{
			this.AnimationTree.Set("parameters/AttackOneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
			this.Attacking = true;
		}
	}

	private void ApplyGravity(float gravity, double delta)
	{
		this.Velocity = new Vector3(this.Velocity.X, this.Velocity.Y - gravity * (float)delta, this.Velocity.Z);
	}
}
