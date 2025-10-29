using Godot;
using GodotRTSCourse.Scripts.Game.EnumAndConsts;
using System;

public partial class EndScreen : Panel
{
    private Label headerText;

    private Button menuButton;

    public override void _Ready()
    {
        this.headerText = this.GetNode<Label>("HeaderText");

        this.menuButton = this.GetNode<Button>("MenuButton");
        this.menuButton.Pressed += this.OnMenuButtonPressed;
    }

    public void SetScreen(string winningTeam)
    {
        this.Visible = true;
        this.headerText.Text = $"{winningTeam} team has won!!!";
    }

    private void OnMenuButtonPressed()
    {
        this.GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
    }

    public override void _ExitTree()
    {
        this.menuButton.Pressed -= this.OnMenuButtonPressed;
    }
}
