using Godot;
using System;

namespace GodotRTSCourse.Scripts.Game;

public partial class Menu : Control
{
    private Button playButton;
    private Button quitButton;

    public override void _Ready()
    {
        this.playButton = this.GetNode<Button>("PlayButton");
        this.quitButton = this.GetNode<Button>("QuitButton");

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

    public override void _ExitTree()
    {
        this.playButton.Pressed -= this.OnPlayButtonPressed;
        this.quitButton.Pressed -= this.OnQuitButtonPressed;
    }
}
