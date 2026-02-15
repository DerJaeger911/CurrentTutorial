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
	private WeaponData currentWeaponData;
	List<WeaponData> collectedWeapons;
	List<ShieldData> collectedShields;
	List<StyleData> collectedStyles;
	private ShieldData currentShieldData;
	private StyleData currentStyleData;
	private int currentWeaponIndex = 0;
	private int currentShieldIndex = 0;
	private int currentStyleIndex = 0;
	private BoneAttachment3D rightHand;
	private BoneAttachment3D leftHand;
	private BoneAttachment3D head;
	private Hud hud;

	public int CurrentWeaponIndex { get => this.currentWeaponIndex; set => this.currentWeaponIndex = value; }
	public int CurrentShieldIndex { get => this.currentShieldIndex; set => this.currentShieldIndex = value; }
	public int CurrentStyleIndex { get => this.currentStyleIndex; set => this.currentStyleIndex = value; }
	public List<WeaponData> CollectedWeapons { get => this.collectedWeapons; set => this.collectedWeapons = value; }
	public List<ShieldData> CollectedShields { get => this.collectedShields; set => this.collectedShields = value; }
	public List<StyleData> CollectedStyles { get => this.collectedStyles; set => this.collectedStyles = value; }

	override public void _Ready()
	{
		base._Ready();
		this.hud = (Hud)this.GetTree().GetFirstNodeInGroup("HUD");
		this.hud.Setup(this.Health);
		this.Skin = this.GetNode<Node3D>("PlayerSkin");
		this.rightHand = this.Skin.GetNode<BoneAttachment3D>("Rogue/Rig/Skeleton3D/RightHand");
		this.leftHand = this.Skin.GetNode<BoneAttachment3D>("Rogue/Rig/Skeleton3D/LeftHand");
		this.head = this.Skin.GetNode<BoneAttachment3D>("Rogue/Rig/Skeleton3D/Head");
		this.CollectedWeapons = new List<WeaponData>() { Equipment.Instance.Weapons[WeaponEnum.Sword] };
		this.CollectedShields = new List<ShieldData>() { Equipment.Instance.Shields[ShieldEnum.Round] };
		this.CollectedStyles = new List<StyleData>() { Equipment.Instance.Styles[StyleEnum.Duckhat] };
		this.currentWeaponData = this.CollectedWeapons[this.CurrentWeaponIndex];
		this.currentShieldData = this.CollectedShields[this.CurrentShieldIndex];
		this.currentStyleData = this.CollectedStyles[this.CurrentStyleIndex];
		this.camera = this.GetNode<Camera3D>("CameraController/Camera3D");
		this.Equip(this.currentWeaponData, this.rightHand);
		this.Equip(this.currentShieldData, this.leftHand);
		this.Equip(this.currentStyleData, this.head);
		this.InvincibilityTimer = this.GetNode<Timer>("Timers/InvincibleTimer");
		this.InvincibilityTimer.Timeout += () => this.Invincible = false;
	}

	public override void _PhysicsProcess(Double delta)
	{
		base._PhysicsProcess(delta);
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
				this.CurrentWeaponIndex = this.SwitchItem(this.CurrentWeaponIndex, this.CollectedWeapons, this.rightHand);
			}

			if (Input.IsActionJustPressed("switch_shield"))
			{
				this.CurrentShieldIndex = this.SwitchItem(this.CurrentShieldIndex, this.CollectedShields, this.leftHand);
			}
		}

		if (Input.IsActionJustPressed("menu"))
		{
			PauseLogic.Instance.TogglePause(true);
		}
	}

	private int SwitchItem<T>(int currentIndex,List<T> dataSource, BoneAttachment3D slot)
	where T : ItemDataBase
	{
		int nextIntIndex = (currentIndex + 1) % dataSource.Count;

		var data = dataSource[nextIntIndex];

		if (data != null)
		{
			this.Equip(data, slot);
		}

		return nextIntIndex;
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
			double playerYRotation = Mathf.RotateToward(this.Skin.Rotation.Y, targetAngle, delta * 6);
			this.Skin.Rotation = new Vector3(this.Skin.Rotation.X, (float)playerYRotation, this.Skin.Rotation.Z);
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
			this.CurrentWeaponNode.PlaySound();
		}
	}

	public void Unequip()
	{
		foreach(Node child in this.head.GetChildren())
		{
			child.QueueFree();
		}
	}
	protected override void DeathLogic()
	{
		this.GetTree().Quit();
	}
}
