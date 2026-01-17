using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Constants;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcozysandbox.Scripts.Entities.Player;

public partial class AnimationControler : AnimationPlayer
{
	private List<string> animations = [];
	private IReadOnlyList<string> animationKeys = ToolConstants.All;
	private PlayerAudio playerAudio;

	public override void _Ready()
	{
		this.playerAudio = this.GetNode<PlayerAudio>("../Audio");
		this.animations =
	ToolConstants.All.SelectMany(k => new[] {$"{k}_down", $"{k}_left", $"{k}_right", $"{k}_up"}).ToList();

		foreach (string animName in this.animations)
		{
			this.AddMethodKeyToAnimation(animName, 0.2);
		}
		this.AnimationFinished += this.OnAnimationFinished;
	}

	private void AddMethodKeyToAnimation(string animName, double time)
	{
		if (!this.HasAnimation(animName))
		{
			return;
		}

		Animation anim = this.GetAnimation(animName);

		for (int i = anim.GetTrackCount() - 1; i >= 0; i--)
		{
			if (anim.TrackGetType(i) == Animation.TrackType.Method)
			{
				anim.RemoveTrack(i);
			}
		}

		int trackIndex = anim.AddTrack(Animation.TrackType.Method);

		Node rootNode = this.GetNode(this.RootNode);
		anim.TrackSetPath(trackIndex, rootNode.GetPathTo(this.playerAudio));

		Godot.Collections.Dictionary methodData = [];

		switch (animName)
		{
			case string a when a.StartsWith(ToolConstants.Axe) || a.StartsWith(ToolConstants.Sword):
				methodData.Add("method", "PlayAxeSwordAudio");
				break;

			case string a when a.StartsWith(ToolConstants.Hoe):
				methodData.Add("method", "PlayHoeAudio");
				break;

			case string a when a.StartsWith(ToolConstants.Fish):
				methodData.Add("method", "PlayFishAudio");
				break;

			case string a when a.StartsWith(ToolConstants.Water):
				methodData.Add("method", "PlayWaterAudio");
				break;

			default:
				break;
		}

		methodData.Add("args", new Godot.Collections.Array());

		anim.TrackInsertKey(trackIndex, (float)time, methodData);
	}

	private void OnAnimationFinished(StringName animName)
	{
		string animationName = animName;
		if (ToolConstants.All.Any(key => animationName.StartsWith(key)))
		{
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.ToolAnimationFinished);
		}
	}
}