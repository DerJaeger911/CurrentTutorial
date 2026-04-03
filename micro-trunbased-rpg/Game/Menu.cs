using Godot;
using System;
using System.Runtime.CompilerServices;

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
		this.GetTree().ChangeSceneToFile("res://Game/Battle.tscn");
	}

	private void OnQuitButtonPressed()
	{
		this.GetTree().Quit();
	}

	public override void _ExitTree()
	{
		this.playButton.Pressed -= this.OnPlayButtonPressed;
		this.quitButton.Pressed -= this.OnQuitButtonPressed;
	}
}
