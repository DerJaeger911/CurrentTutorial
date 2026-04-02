using Godot;
using Godot.Collections;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace MicroTrunbasedRPG.Game;

public partial class Character : Node2D
{
	[Export]
	private bool isPlayer;
	[Export]
	private int currentHealth;
	[Export]
	private int maxHealth = 100;
	[Export]
	private Array<CombatAction> combatActions;
	[Export]
	private bool faceLeft;
	[Export]
	private Texture2D texture;

	private float targetScale = 1.0f;

	private HealthBar healthBar;

	private CharacterVisuals sprite;

	private AudioStreamPlayer audioPlayer;
	private AudioStream takeDamageSfx = GD.Load<AudioStream>("res://Assets/Audio/take_damage.wav");
	private AudioStream healSfx = GD.Load<AudioStream>("res://Assets/Audio/heal.wav");

	public Array<CombatAction> CombatActions { get => this.combatActions; set => this.combatActions = value; }
	public Int32 CurrentHealth { get => this.currentHealth; set => this.currentHealth = value; }
	public Int32 MaxHealth { get => this.maxHealth; set => this.maxHealth = value; }
	public Boolean IsPlayer { get => this.isPlayer; set => this.isPlayer = value; }

	override public void _Ready()
	{
		this.sprite = this.GetNode<CharacterVisuals>("Sprite");
		this.audioPlayer = this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		this.healthBar = this.GetNode<HealthBar>("HealthBar");

		this.sprite.Init(this.faceLeft, this.texture);

		if (this.faceLeft)
		{
			this.sprite.FlipH = true;
		}

		if (this.texture != null)
		{
			this.sprite.Texture = this.texture;
		}

		if (this.CurrentHealth == 0)
		{
			this.CurrentHealth = this.MaxHealth;
		}

		this.healthBar.UpdateMaxHealth(this.MaxHealth);
		this.healthBar.UpdateHealthBar(this.CurrentHealth);
	}

	public override void _Process(Double delta)
	{
		float X = Mathf.Lerp(this.Scale.X, this.targetScale, (float)delta * 10);
		float Y = Mathf.Lerp(this.Scale.Y, this.targetScale, (float)delta * 10);

		this.Scale = new Vector2(X, Y);
	}

	public void CastCombatAction(CombatAction action,Character opponent)
	{
		if(action == null)
		{
			return;
		}

		if(action.MeleeDamage > 0)
		{
			opponent.TakeDamage(action.MeleeDamage);
		}

		if(action.HealAmount > 0)
		{
			this.Heal(action.HealAmount);
		}
	}

	public void BeginTurn()
	{
		this.targetScale = 1.1f;
	}

	public void EndTurn()
	{
		this.targetScale = 0.9f;
	}	

	private void TakeDamage(int damage)
	{
		this.CurrentHealth -= damage;
		this.healthBar.UpdateHealthBar(this.CurrentHealth);
		this.PlayAudio(this.takeDamageSfx);
		_= this.sprite.DamageVisuals();
	}

	private void Heal(int healAmount)
	{
		this.CurrentHealth += healAmount;
		this.CurrentHealth = Math.Clamp(this.CurrentHealth, 0, this.MaxHealth);
		this.healthBar.UpdateHealthBar(this.CurrentHealth);
		this.PlayAudio(this.healSfx, -10);

	}

	private void PlayAudio(AudioStream stream, float volumeDb = 0.0f)
	{
		this.audioPlayer.VolumeDb = volumeDb;
		this.audioPlayer.Stream = stream;
		this.audioPlayer.Play();
	}
}
