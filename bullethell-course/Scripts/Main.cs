using Godot;
using System;

namespace bullethellcourse.Scripts;

public partial class Main : Node2D
{
	private double elapsedTime;

	private Label elapsedTimeText;

    private Button retryButton;

    private Panel endScreen;

    private Label endScreenText;

    public override void _Ready()
    {
        this.elapsedTimeText = this.GetNode<Label>("CanvasLayer/ElapsedTimeText");
        this.retryButton = this.GetNode<Button>("CanvasLayer/EndScreen/RetryButton");
        this.endScreen = this.GetNode<Panel>("CanvasLayer/EndScreen");
        this.endScreenText = this.GetNode<Label>("CanvasLayer/EndScreen/EndScreenText");
		this.elapsedTime = 0;

        this.endScreen.Visible = false;

        this.retryButton.Pressed += this.OnRetryButtonPressed;
    }

    public override void _Process(Double delta)
    {
        this.elapsedTime += delta;
        this.elapsedTimeText.Text = this.elapsedTime.ToString("F2");
    }

    public void SetGameOver()
    {
        Engine.TimeScale = 0;
		this.endScreen.Visible = true;
        this.endScreenText.Text = $"You survived for {this.elapsedTime.ToString("F2")}";
    }

    private void OnRetryButtonPressed()
    {
        Engine.TimeScale = 1;
        this.GetTree().ReloadCurrentScene();
    }
}
