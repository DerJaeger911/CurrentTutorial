using Godot;
using System;

public partial class Menu : Control
{
	private Button playButton;

	private Button quitButton;

    public override void _Ready()
    {
        this.playButton = this.GetNode<Button>("VBoxContainer/PlayButton");
		this.quitButton = this.GetNode<Button>("VBoxContainer/QuitButton");

		this.playButton.Pressed += this.OnPlayButtonPressed;
		this.quitButton.Pressed += this.OnQuitButtonPressed;
	}

	private void OnPlayButtonPressed()
	{
		this.GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}

	private void OnQuitButtonPressed()
	{
		this.GetTree().Quit();
	}
}
