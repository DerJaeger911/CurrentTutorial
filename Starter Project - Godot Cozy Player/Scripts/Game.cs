using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Enemies;
using Godot;
using System;

public partial class Game : Node2D
{
	[Export]
	private Gradient dayTimeGradient;
	[Export]
	private Curve musicCurve;
	private CanvasModulate canvasModulate;
	private Timer dayTimer;
	private CanvasLayer canvasLayer;
	private ColorRect dayTransition;
	private AudioStreamPlayer music;

	private double daytimePoint = 0;
	private bool fadeOut;

	public override void _Ready()
	{
		this.canvasLayer = this.GetNode<CanvasLayer>("CanvasLayer");
		this.dayTransition = this.canvasLayer.GetNode<ColorRect>("DayTransition");
		this.canvasModulate = this.GetNode<CanvasModulate>("CanvasModulate");
		this.dayTimer = this.GetNode<Timer>("DayTimer");
		this.music = this.GetNode<AudioStreamPlayer>("Music");
		SignalBus.Instance.ToolInteract += this.OnToolInteract;

	}

	public override void _Process(Double delta)
	{
		this.daytimePoint = 1 - (this.dayTimer.TimeLeft / this.dayTimer.WaitTime);
		this.canvasModulate.Color = this.dayTimeGradient.Sample((float)this.daytimePoint);
		if (!this.fadeOut)
		{
			this.music.VolumeDb = this.musicCurve.Sample((float)this.daytimePoint);
		}

		if (Input.IsActionJustPressed("ui_focus_next"))
		{
			this.DayRestart();
		}
	}

	private void DayRestart()
	{
		this.fadeOut = true;
		Tween tween = this.GetTree().CreateTween();
		tween.Parallel().TweenProperty(this.music, "volume_db", -80.0f, 1.0f);
		tween.Parallel().TweenProperty(this.dayTransition.Material, "shader_parameter/progress", 1, 1);
		tween.TweenInterval(0.5);
		tween.TweenCallback(Callable.From(this.ResetLevel));
		tween.Parallel().TweenProperty(this.music, "volume_db", this.musicCurve.Sample((float)this.daytimePoint), 1.0f);
		tween.Parallel().TweenProperty(this.dayTransition.Material, "shader_parameter/progress", 0.0f, 1.0f);
		tween.TweenCallback(Callable.From(new Action(() => { this.fadeOut = false; })));
	}

	private void ResetLevel()
	{
		this.dayTimer.Start();
		this.music.Play();
		foreach(Tree tree in this.GetTree().GetNodesInGroup("Trees"))
		{
			tree.Reset();
		}
	}

	private void OnToolInteract(int tool, Vector2 position)
	{
		GD.Print(ToolConstants.All[tool]);
		GD.Print(position);
		switch (ToolConstants.All[tool])
		{
			case ToolConstants.Axe:
				foreach (Tree tree in this.GetTree().GetNodesInGroup("Trees"))
				{
					if (tree.Position.DistanceTo(position) < 16)
					{
						tree.TakeDamage(1);
					}
				}
				break;
			case ToolConstants.Sword:
				foreach (Enemy enemy in this.GetTree().GetNodesInGroup("Enemies"))
				{
					if (enemy.Position.DistanceTo(position) < 12)
					{
						enemy.TakeDamage(1);
					}
				}
				break;
		}
	}

	public override void _ExitTree()
	{
		SignalBus.Instance.ToolInteract -= this.OnToolInteract;
	}
}
