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
	[Export]
	private float runSpeed = 8;
	[Export]
	private float defendSpeed = 2;
	[Export]
	private int jumpCount = 1;
	[Export]
	private int maxJumpCount = 2;

	private bool defending;

	private AnimationNodeStateMachinePlayback moveStateMachine;
	private AnimationNodeAnimation attackAnimation;

	public Single BaseSpeed { get => this.baseSpeed; set => this.baseSpeed = value; }
    public Vector2 MovementInput { get; set; }
    public Single JumpVelocity { get; set; }
    public Single JumpGravity { get; set; }
    public Single FallGravity { get; set; }
    public AnimationTree AnimationTree { get; set; }
    public Boolean Attacking { get; set; }
	public Single RunSpeed { get => this.runSpeed; set => this.runSpeed = value; }
	public Single DefendSpeed { get => this.defendSpeed; set => this.defendSpeed = value; }
	public Int32 JumpCount { get => this.jumpCount; set => this.jumpCount = value; }
	public Int32 MaxJumpCount { get => this.maxJumpCount; set => this.maxJumpCount = value; }

	public Boolean Defending 
	{
		get => this.defending;
		set
		{
			if (this.defending != value)
			{
				this.defending = value;
				this.DefendToggle(value);
			}
		}
	}

    public override void _Ready()
    {
        this.JumpVelocity = ((2 * this.jumpHeight) / this.jumpTimeToPeak) * -1;
		this.JumpGravity = ((-2 * this.jumpHeight) / (this.jumpTimeToPeak * this.jumpTimeToPeak)) * -1;
		this.FallGravity = ((-2 * this.jumpHeight) / (this.jumpTimeToDescent * this.jumpTimeToDescent)) * -1;
		this.AnimationTree = this.GetNode<AnimationTree>("AnimationTree");
		var root = this.AnimationTree.TreeRoot as AnimationNodeBlendTree;
		this.moveStateMachine = (AnimationNodeStateMachinePlayback)this.AnimationTree.Get("parameters/MoveStateMachine/playback");
		this.attackAnimation = root.GetNode("AttackAnimation") as AnimationNodeAnimation;
		this.AnimationTree.AnimationFinished += this.OnAnimationFinished;
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

		var itemScene = data.Scene.Instantiate<Node3D>();
		slot.AddChild(itemScene);

		if (data is WeaponData weapon && itemScene is Weapon wScript)
		{
			wScript.Setup(weapon.Animation, weapon.Damage, weapon.Range, this);
			this.attackAnimation.Animation = weapon.Animation;
		}
		else if (data is ShieldData shield && itemScene is Shield sScript)
		{
			sScript.Setup(shield.Defense);
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		if (animName == this.attackAnimation.Animation)
		{
			this.Attacking = false;
		}
	}

	public void DefendToggle(bool defending)
	{
		var tween = this.GetTree().CreateTween();
		float defendingValue = defending ? 1.0f : 0.0f;
		tween.TweenMethod(Callable.From<float>(this.DefendChange), 1 - defendingValue, defendingValue, 0.25f );
	}

	private void DefendChange(float value)
	{
		this.AnimationTree.Set("parameters/DefendBlend/blend_amount", value);
	}
}
