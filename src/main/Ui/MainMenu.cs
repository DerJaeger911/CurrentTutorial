using Godot;
using System;

public partial class MainMenu : Node2D
{
    private Button quitButton;

    public override void _Ready()
    {
        this.quitButton = this.GetNode<Button>("VBoxContainer/Quit");
        this.quitButton.Pressed += this.Quit;
    }

    public void Quit()
    {
        this.GetTree().Quit();
    }

    public override void _ExitTree()
    {
        this.quitButton.Pressed -= this.Quit;
    }
}
