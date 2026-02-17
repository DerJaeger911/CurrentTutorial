using Adventuregame.Scripts.Items;
using Godot;
using System;

public partial class TransitionLayer : CanvasLayer
{
	private ColorRect colorRect;
	override public void _Ready()
	{
		this.colorRect = this.GetNode<ColorRect>("ColorRect");
		this.colorRect.Modulate = new Color(0, 0, 0, 0);
	}

	public void ChangeScene(string targetLevelPath)
	{
		Tween tween = this.CreateTween();
		tween.TweenProperty(this.colorRect, "modulate", new Color(0, 0, 0, 1), 0.5);
		tween.TweenCallback(Callable.From(() => this.OpenScene(targetLevelPath)));
		tween.TweenProperty(this.colorRect, "modulate", new Color(0, 0, 0, 0), 0.5);

	}

	private void OpenScene(string targetLevelPath)
	{
		this.GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, targetLevelPath);
	}
}
