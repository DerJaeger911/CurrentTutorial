using Godot;
using System;

public partial class Game : Node2D
{
	[Export]
	private Gradient dayTimeGradient;
    private CanvasModulate canvasModulate;
    private Timer dayTimer;
    private CanvasLayer canvasLayer;
    private ColorRect dayTransition;

    public override void _Ready()
    {
        this.canvasLayer = this.GetNode<CanvasLayer>("CanvasLayer");
        this.dayTransition = this.canvasLayer.GetNode<ColorRect>("DayTransition");
        this.canvasModulate = this.GetNode<CanvasModulate>("CanvasModulate");
        this.dayTimer = this.GetNode<Timer>("DayTimer");
    }

    public override void _Process(Double delta)
    {
        var daytimePoint = 1 - (this.dayTimer.TimeLeft / this.dayTimer.WaitTime);
        this.canvasModulate.Color = this.dayTimeGradient.Sample((float)daytimePoint);

        if (Input.IsActionJustPressed("ui_focus_next"))
        {
            this.DayRestart();
        }
    }

    private void DayRestart()
    {
        var tween = this.GetTree().CreateTween();
        tween.TweenProperty(this.dayTransition.Material, "shader_parameter/progress", 1, 1);
		tween.TweenInterval(0.5);
		tween.TweenCallback(Callable.From(this.ResetLevel));
		tween.TweenProperty(this.dayTransition.Material, "shader_parameter/progress", 0, 1);
	}

    private void ResetLevel()
    {
        this.dayTimer.Start();
    }
}
