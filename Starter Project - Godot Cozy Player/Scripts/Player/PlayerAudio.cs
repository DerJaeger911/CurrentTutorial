using Dcozysandbox.Scripts.Constants;
using Godot;
using System;

public partial class PlayerAudio : Node
{
	private AudioStreamPlayer hoeSound;
	private AudioStreamPlayer axeSwordSound;
	private AudioStreamPlayer fishSound;
	private AudioStreamPlayer waterSound;
	private AudioStreamPlayer walkSound;

	public override void _Ready()
	{
		this.hoeSound = this.GetNode<AudioStreamPlayer>("HoeSound");
		this.axeSwordSound = this.GetNode<AudioStreamPlayer>("AxeSwordSound");
		this.fishSound = this.GetNode<AudioStreamPlayer>("FishSound");
		this.waterSound = this.GetNode<AudioStreamPlayer>("WaterSound");
		this.walkSound = this.GetNode<AudioStreamPlayer>("WalkSound");

	}

	public void PlayAxeSwordAudio()
	{
		this.axeSwordSound.Play();
	}

	public void PlayHoeAudio()
	{
		this.hoeSound.Play();
	}

	public void PlayFishAudio()
	{
		this.fishSound.Play();
	}

	public void PlayWaterAudio()
	{
		this.waterSound.Play();
	}

	public void PlayWalkSound()
	{
		if (!this.walkSound.Playing)
		{
			this.walkSound.PitchScale = (float)GD.RandRange(0.9, 1.1);
			this.walkSound.Play();
		}
	}
}
