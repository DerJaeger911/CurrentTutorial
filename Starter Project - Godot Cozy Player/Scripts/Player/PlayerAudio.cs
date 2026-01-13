using Dcozysandbox.Scripts.Constants;
using Godot;
using System;

public partial class PlayerAudio : Node
{
	private AudioStreamPlayer hoeSound;
	private AudioStreamPlayer axeSwordSound;
	private AudioStreamPlayer fishSound;
	private AudioStreamPlayer waterSound;


	public override void _Ready()
	{
		this.hoeSound = this.GetNode<AudioStreamPlayer>("HoeSound");
		this.axeSwordSound = this.GetNode<AudioStreamPlayer>("AxeSwordSound");
		this.fishSound = this.GetNode<AudioStreamPlayer>("FishSound");
		this.waterSound = this.GetNode<AudioStreamPlayer>("WaterSound");

	}

	public void PlayAudio(string tool)
	{
		switch (tool)
		{
			case ToolConstants.Hoe:
				this.hoeSound.Play();
				break;
			case ToolConstants.Axe:
				this.axeSwordSound.Play();
				break;
			case ToolConstants.Fish:
				this.fishSound.Play();
				break;
			case ToolConstants.Water:
				this.waterSound.Play();
				break;
		}
	}
}
