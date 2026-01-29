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

	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback moveStateMachine;

	public Single BaseSpeed { get => this.baseSpeed; set => this.baseSpeed = value; }
    public Vector2 MovementInput { get; set; }
    public Single JumpVelocity { get => this.jumpVelocity; set => this.jumpVelocity = value; }
    public Single JumpGravity { get => this.jumpGravity; set => this.jumpGravity = value; }
    public Single FallGravity { get => this.fallGravity; set => this.fallGravity = value; }

    public override void _Ready()
    {
        this.JumpVelocity = ((2 * this.jumpHeight) / this.jumpTimeToPeak) * -1;
		this.JumpGravity = ((-2 * this.jumpHeight) / (this.jumpTimeToPeak * this.jumpTimeToPeak)) * -1;
		this.FallGravity = ((-2 * this.jumpHeight) / (this.jumpTimeToDescent * this.jumpTimeToDescent)) * -1;
		this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
		this.moveStateMachine = (AnimationNodeStateMachinePlayback)this.animationTree.Get("parameters/MoveStateMachine/playback");
		GD.Print(this.moveStateMachine);
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
		var itemScene = data.Scene.Instantiate<Weapon>();
		slot.AddChild(itemScene);

		if (data is WeaponData weapon)
		{
			GD.Print("Waffe ausgerüstet! Schaden: " + weapon.Damage);
			itemScene.Setup(weapon.Animation, weapon.Damage, weapon.Range, this);
		}
		else if (data is ShieldData shield)
		{
			GD.Print("Schild ausgerüstet!");
		}
		else if (data is StyleData style)
		{
			GD.Print("Geiler Style");
		}
	}
}
