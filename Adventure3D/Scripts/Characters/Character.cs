using Adventuregame.Scripts.GlobalData.Enums;
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
	[Export]
	private int health = 5;

	private float squashAndStretch = 1;
	private bool hasHit;
	private bool defending;

	private AnimationNodeStateMachinePlayback moveStateMachine;
	private AnimationNodeAnimation attackAnimation;
	private AudioStreamPlayer3D hitSound;
	public Weapon CurrentWeaponNode {  get; private set; }
	public Shield CurrentShieldNode {  get; private set; }

	public Single BaseSpeed { get => this.baseSpeed; set => this.baseSpeed = value; }
	public Node3D Skin { get; set; }
	public Vector2 MovementInput { get; set; }
    public Single JumpVelocity { get; set; }
    public Single JumpGravity { get; set; }
    public Single FallGravity { get; set; }
    public AnimationTree AnimationTree { get; set; }
    public Boolean Attacking { get; set; }
	public Boolean Invincible { get; set; }
	public Single RunSpeed { get => this.runSpeed; set => this.runSpeed = value; }
	public Single DefendSpeed { get => this.defendSpeed; set => this.defendSpeed = value; }
	public Int32 JumpCount { get => this.jumpCount; set => this.jumpCount = value; }
	public Int32 MaxJumpCount { get => this.maxJumpCount; set => this.maxJumpCount = value; }
	public Int32 Health 
	{ 
		get => this.health;
		set 
		{
			this.health = value;
			if (this is Player.Player)
			{
				Hud hud = (Hud)this.GetTree().GetFirstNodeInGroup("HUD");
				hud.SetHealth(value);
			}
		} 
	}
	protected Timer InvincibilityTimer { get; set; }

	public readonly WeaponEnum[] AllWeapons = Enum.GetValues<WeaponEnum>();

	public readonly ShieldEnum[] AllShields = Enum.GetValues<ShieldEnum>();

	public readonly StyleEnum[] AllStyles = Enum.GetValues<StyleEnum>();

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

    public Single SquashAndStretch
	{
		get => this.squashAndStretch;
		set
		{
			this.squashAndStretch = value;
			float negative = 1 + (1 - value);
			this.Skin.Scale = new Vector3(negative, value, negative);
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
		this.hitSound = this.GetNode<AudioStreamPlayer3D>("HitSound");
		this.AnimationTree.AnimationFinished += this.OnAnimationFinished;

	}

	override public void _PhysicsProcess(double delta)
	{
		this.AttackLogic();
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
			this.CurrentWeaponNode = wScript;
			this.CurrentWeaponNode.SetSound(weapon.Audio);
		}
		else if (data is ShieldData shield && itemScene is Shield sScript)
		{
			sScript.Setup(shield.Defense);
			this.CurrentShieldNode = sScript;
			this.CurrentShieldNode.Position = new Vector3(0, 0, 0.2f);
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		if (animName == this.attackAnimation.Animation)
		{
			this.Attacking = false;
			this.hasHit = false;
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

	public void AttackLogic()
	{
		if (this.Attacking && !this.hasHit)
		{
			GodotObject collider = this.CurrentWeaponNode.GetCollider();
			if (collider is not null && collider != this && collider is Character character)
			{
				character.Hit(this.CurrentWeaponNode.Damage);
				this.hasHit = true;
			}
		}
	}

	protected virtual void DeathLogic()
	{
		GD.Print("Die allready!");
	}

	public void Hit(int damage)
	{
        if (this.Invincible)
        {
			GD.Print("Invincible");
			return;
        }

        if (this.CurrentShieldNode is not null && this.Defending)
		{
			this.CurrentShieldNode.Flash();
			this.Health -= (int)(damage * this.CurrentShieldNode.Defense);
			this.CurrentShieldNode.PlaySound();
		}
		else
		{
			this.Health -= damage;
			this.hitSound.Play();
		}

		if (this.InvincibilityTimer != null)
		{
			this.Invincible = true;
			this.InvincibilityTimer.Start();
		}

		this.DoSquashAndStrecth(1.2f, 0.2f);
		if (this.Health <= 0)
		{
			this.DeathLogic();
		}
	}

	private void DoSquashAndStrecth(float value, float duration)
	{
		Tween tween = this.CreateTween();
		tween.TweenProperty(this, nameof(this.SquashAndStretch), value, duration);
		tween.TweenProperty(this, nameof(this.SquashAndStretch), 1, duration * 1.8f).SetEase(Tween.EaseType.Out);

	}

	public void ApplyGravity(float gravity, double delta)
	{
		this.Velocity = new Vector3(this.Velocity.X, this.Velocity.Y - gravity * (float)delta, this.Velocity.Z);
	}

	public override void _ExitTree()
	{
		this.AnimationTree.AnimationFinished -= this.OnAnimationFinished;
	}
}
