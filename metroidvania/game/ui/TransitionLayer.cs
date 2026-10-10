using Godot;
using System;
using System.Runtime;

public partial class TransitionLayer : CanvasLayer
{
	public static TransitionLayer Instance { get; private set; }
	private ColorRect colorRect;
	private Color modulate;

	public override void _Ready()
	{
		if (Instance != null)
		{
			GD.PrintErr("Multiple instances of TransitionLayer detected. This is not allowed.");
			this.QueueFree();
			return;
		}
		Instance = this;

        this.colorRect = this.GetNode<ColorRect>("ColorRect");
		this.modulate = this.colorRect.Modulate;
		this.modulate.A = 0.0f;
		this.UpdateTransitionLayer();
	}

	private void UpdateTransitionLayer()
	{
		this.colorRect.Modulate = this.modulate;
	}

	public void Transition(LevelEnums targetLevel, LevelEnums currentLevel)
	{
		Tween tween = this.CreateTween();
		tween.TweenProperty(this.colorRect, "modulate:a", 1.0, 0.8);
		tween.TweenInterval(0.5);
		tween.TweenCallback(Callable.From(() => this.ChangeScene(targetLevel, currentLevel)));
		tween.TweenProperty(this.colorRect, "modulate:a", 0.0, 0.8);
	}

	private void ChangeScene(LevelEnums targetLevel, LevelEnums currentLevel)
	{
		if (this.GetTree().CurrentScene != null)
		{
			this.GetTree().CurrentScene.QueueFree();
			var scene = GD.Load<PackedScene>(Data.Instance.LevelPaths[targetLevel]).Instantiate();
			this.GetTree().Root.AddChild(scene);
			this.GetTree().CurrentScene = scene;
			Data.Instance.CurrentLevel = targetLevel;
		}
	}
}
