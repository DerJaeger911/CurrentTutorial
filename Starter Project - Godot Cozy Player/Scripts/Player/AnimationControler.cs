using Godot;
using System;
using System.Collections.Generic;

public partial class AnimationControler : AnimationPlayer
{
	private List<string> animations = [];
	private List<string> animationKeys = ["hoe", "axe", "sword", "fish", "water"];
	private PlayerAudio playerAudio;

	public override void _Ready()
	{
		this.playerAudio = this.GetNode<PlayerAudio>("../Audio");
		foreach (string animationKey in this.animationKeys)
		{
			this.animations.AddRange([animationKey + "_down", animationKey + "_left", animationKey + "_right", animationKey + "_up"]);
		}

		foreach (string animName in this.animations)
		{
			this.AddMethodKeyToAnimation(animName, 0.2);
		}
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
			case string a when a.StartsWith("axe") || a.StartsWith("sword"):
				methodData.Add("method", "PlayAxeSwordAudio");
				break;

			case string a when a.StartsWith("hoe"):
				methodData.Add("method", "PlayHoeAudio");
				break;

			case string a when a.StartsWith("fish"):
				methodData.Add("method", "PlayFishAudio");
				break;

			case string a when a.StartsWith("water"):
				methodData.Add("method", "PlayWaterAudio");
				break;

			default:
				break;
		}

		methodData.Add("args", new Godot.Collections.Array());

		anim.TrackInsertKey(trackIndex, (float)time, methodData);
	}
}