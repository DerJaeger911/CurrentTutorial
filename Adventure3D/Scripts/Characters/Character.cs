using Adventuregame.Scripts.GlobalData.ObjectDataClasses;
using Adventuregame.Scripts.Items;
using Godot;
using System;

namespace Adventuregame.Scripts.Characters;

public partial class Character : CharacterBody3D
{
	[Export]
	private float baseSpeed = 4;
	[Export]
	private float jumpHeight = 2.25f;
	[Export]
	private float jumpTimeToPeak = 0.4f;
	[Export]
	private float jumpTimeToDescent = 0.3f;
	private float jumpVelocity;
	private float jumpGravity;
	private float fallGravity;
	private bool attacking;

	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback moveStateMachine;
	private AnimationNodeAnimation attackAnimation;

	public Single BaseSpeed { get => this.baseSpeed; set => this.baseSpeed = value; }
    public Vector2 MovementInput { get; set; }
    public Single JumpVelocity { get => this.jumpVelocity; set => this.jumpVelocity = value; }
    public Single JumpGravity { get => this.jumpGravity; set => this.jumpGravity = value; }
    public Single FallGravity { get => this.fallGravity; set => this.fallGravity = value; }
    public AnimationTree AnimationTree { get => this.animationTree; set => this.animationTree = value; }
    public Boolean Attacking { get => this.attacking; set => this.attacking = value; }

    public override void _Ready()
    {
        this.JumpVelocity = ((2 * this.jumpHeight) / this.jumpTimeToPeak) * -1;
		this.JumpGravity = ((-2 * this.jumpHeight) / (this.jumpTimeToPeak * this.jumpTimeToPeak)) * -1;
		this.FallGravity = ((-2 * this.jumpHeight) / (this.jumpTimeToDescent * this.jumpTimeToDescent)) * -1;
		this.AnimationTree = this.GetNode<AnimationTree>("AnimationTree");
		var root = this.AnimationTree.TreeRoot as AnimationNodeBlendTree;
		GD.Print(root);
		this.moveStateMachine = (AnimationNodeStateMachinePlayback)this.AnimationTree.Get("parameters/MoveStateMachine/playback");
		this.attackAnimation = root.GetNode("AttackAnimation") as AnimationNodeAnimation;
		this.animationTree.AnimationFinished += this.OnAnimationFinished;
	}

	public void SetMoveState(String stateName)
	{
		if(this.moveStateMachine.GetCurrentNode() != stateName)
		{
			this.moveStateMachine.Travel(stateName);
		}
	}

	protected void Equip(ItemDataBase data, BoneAttachment3D slot)
	{
		foreach (Node3D child in slot.GetChildren())
		{
			child.QueueFree();
		}
		

		if (data is WeaponData weapon)
		{
			var itemScene = data.Scene.Instantiate<Weapon>();
			slot.AddChild(itemScene);
			GD.Print("Waffe ausgerüstet! Schaden: " + weapon.Damage);
			itemScene.Setup(weapon.Animation, weapon.Damage, weapon.Range, this);
			this.attackAnimation.Animation = weapon.Animation;
		}
		else if (data is ShieldData shield)
		{
			var itemScene = data.Scene.Instantiate<Shield>();
			slot.AddChild(itemScene);
			itemScene.Setup(shield.Defense);
			GD.Print("Schild ausgerüstet!");
		}
		else if (data is StyleData style)
		{
			GD.Print("Geiler Style");
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		if (animName == this.attackAnimation.Animation)
		{
			this.Attacking = false;
		}
	}
}
