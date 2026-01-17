using Dcozysandbox.Scripts.AutoLoads.Busses;
using Godot;

namespace Dcozysandbox.Scripts.Entities.Player;

public partial class PlayerAudio : Node
{
	private AudioStreamPlayer hoeSound;
	private AudioStreamPlayer axeSwordSound;
	private AudioStreamPlayer fishSound;
	private AudioStreamPlayer waterSound;
	private AudioStreamPlayer walkSound;

	private Player player;

	public override void _Ready()
	{
		this.hoeSound = this.GetNode<AudioStreamPlayer>("HoeSound");
		this.axeSwordSound = this.GetNode<AudioStreamPlayer>("AxeSwordSound");
		this.fishSound = this.GetNode<AudioStreamPlayer>("FishSound");
		this.waterSound = this.GetNode<AudioStreamPlayer>("WaterSound");
		this.walkSound = this.GetNode<AudioStreamPlayer>("WalkSound");
		this.player = (Player)this.GetParent<CharacterBody2D>();
	}

	public void PlayAxeSwordAudio()
	{
		this.axeSwordSound.Play();
		this.ToolInteraction();
	}

	public void PlayHoeAudio()
	{
		this.hoeSound.Play();
		this.ToolInteraction();
	}

	public void PlayFishAudio()
	{
		this.fishSound.Play();
		this.ToolInteraction();
	}

	public void PlayWaterAudio()
	{
		this.waterSound.Play();
		this.ToolInteraction();
	}

	public void PlayWalkSound()
	{
		if (!this.walkSound.Playing)
		{
			this.walkSound.PitchScale = (float)GD.RandRange(0.9, 1.1);
			this.walkSound.Play();
		}
	}

	private void ToolInteraction()
	{
		Vector2 interactionPoint = this.player.Position + this.player.LastDirection * this.player.ToolOffset;
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ToolInteract, this.player.CurrentTool, interactionPoint);
	}
}
