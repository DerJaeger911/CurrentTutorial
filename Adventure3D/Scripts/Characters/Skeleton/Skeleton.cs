using Adventuregame.Scripts.Characters;
using Adventuregame.Scripts.Characters.Player;
using Adventuregame.Scripts.GlobalData;
using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

public partial class Skeleton : Character
{
	[Export]
	private float noticeRadius = 30;
	[Export]
	private float speed = 3;

	private Player player;
	private AnimationTree animationTree;
	private readonly Random range = new Random();
	private float attackRadius = 3;
	private WeaponEnum randomWeapon;
	private Timer attackTimer;
	private RandomNumberGenerator rng = new();


	public override void _Ready()
	{
		base._Ready();
		this.attackTimer = this.GetNode<Timer>("Timers/AttackTimer");
		this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");
		Node3D skinsNode = this.GetNode<Node3D>("Skins");

		Godot.Collections.Array<Node> skins = skinsNode.GetChildren();
		foreach (Node3D child in skins)
		{
			child.Hide();
		}

		this.Skin = (Node3D)skins.PickRandom();
		this.Skin.Show();

		this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
		this.animationTree.AnimPlayer = $"../Skins/{this.Skin.Name}/AnimationPlayer";

		this.randomWeapon = (WeaponEnum)this.range.Next(this.AllWeapons.Length);
		//var selectWeapon = Global.Instance.Weapons[this.randomWeapon];
		var selectWeapon = Equipment.Instance.Weapons[WeaponEnum.Sword];

		this.Equip(selectWeapon, this.Skin.GetNode<BoneAttachment3D>("Rig/Skeleton3D/RightHand"));
		this.attackRadius = this.CurrentWeaponNode.Radius;

		this.attackTimer.Timeout += this.OnAttackTimerTimeOut;
	}

	override public void _PhysicsProcess(Double delta)
	{
		if (this.Health > 0)
		{
			base._PhysicsProcess(delta);
			if (!this.IsOnFloor())
			{
				this.ApplyGravity(this.FallGravity, delta);
			}
			else
			{
				this.Velocity = new Vector3(this.Velocity.X, 0, this.Velocity.Z);
			}
			this.MoveToPlayer(delta);
			this.MoveAndSlide();
		}
	}

	private void MoveToPlayer(double delta)
	{
		if (this.Position.DistanceTo(this.player.Position) < this.noticeRadius)
		{
			Vector3 targetDir = (this.player.Position - this.Position).Normalized();
			Vector2 targetVector2 = new Vector2(targetDir.X, targetDir.Z);
			var targetAngle = -targetVector2.Angle() + float.Pi / 2;
			this.Rotation = new Vector3(this.Rotation.X, Mathf.RotateToward(this.Rotation.Y, targetAngle, (float)delta * 6), this.Rotation.Z);
			if (this.Position.DistanceTo(this.player.Position) > this.attackRadius)
			{
				this.Velocity = new Vector3(targetDir.X, this.Velocity.Y, targetDir.Z) * this.speed;
				this.SetMoveState("Running_A");
			}
			else
			{
				this.Velocity = new Vector3(0, this.Velocity.Y,0);
				this.SetMoveState("Idle");
			}
		}
	}

	protected override void DeathLogic()
	{
		Tween tween = this.CreateTween();
		tween.TweenMethod(Callable.From<float>(this.DeathChange), 0, 1, 0.5f);
	}

	private void DeathChange(float value)
	{
		this.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
		this.attackTimer.Stop();
		this.animationTree.Set("parameters/DeathBlend/blend_amount", value);
	}

	private void OnAttackTimerTimeOut()
	{
		this.attackTimer.WaitTime = this.rng.RandfRange(2, 3.5f);
		if (this.Position.DistanceTo(this.player.Position) < this.attackRadius)
		{
			this.animationTree.Set("parameters/AttackOneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
			this.Attacking = true;
			this.CurrentWeaponNode.PlaySound();
		}
	}
}