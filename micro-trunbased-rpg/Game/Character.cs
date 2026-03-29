using Godot;
using System;

namespace MicroTrunbasedRPG.Game;

public partial class Character : Node2D
{
	[Export]
	private bool isPlayer;
	[Export]
	private int currentHealth;
	[Export]
	private int maxHealth = 100;

	private float targetScale = 1.0f;

	private AudioStreamPlayer audioPlayer;

	private AudioStream takeDamageSfx = GD.Load<AudioStream>("res://Assets/Audio/take_damage.wav");
	private AudioStream healSfx = GD.Load<AudioStream>("res://Assets/Audio/heal.wav");

	override public void _Ready()
	{
		if (this.currentHealth == 0)
		{
			this.currentHealth = this.maxHealth;
		}

		this.audioPlayer = this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}

	public override void _Process(Double delta)
	{
		
	}

	public void CastCombatAction(CombatAction action,Character opponent)
	{

	}

	public void BeginTurn()
	{
		this.targetScale = 1.1f;

		GD.Print(this.Name);
	}

	public void EndTurn()
	{
		this.targetScale = 0.9f;
	}	

	private void TakeDamage(int damage)
	{
		if (this.currentHealth <= 0)
		{
			this.Die();
		}
	}

	private void Heal(int healAmount)
	{
		if(this.currentHealth > this.maxHealth)
		{
			this.currentHealth = this.maxHealth;
		}
		else
		{

		}
	}

	private void Die()
	{
		this.QueueFree();
	}

	private void PlayAudio(AudioStream stream)
	{

	}
}
