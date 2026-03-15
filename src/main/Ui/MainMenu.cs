using Godot;
using System;

public partial class MainMenu : Node2D
{
    private Button quitButton;
    private Button startButton;

    public override void _Ready()
    {
        this.quitButton = this.GetNode<Button>("VBoxContainer/Quit");
		this.startButton = this.GetNode<Button>("VBoxContainer/StartGame");
		this.quitButton.Pressed += this.Quit;
		this.startButton.Pressed += this.Start;
	}

    public void Start()
    {
        Game game = (Game)ResourceLoader.Load<PackedScene>("res://src/main/game.tscn").Instantiate();
        HexTileMap map = game.GetNode<HexTileMap>("HexTileMap");

        map.Width = (int)this.GetNode<SpinBox>("VBoxContainer/HBoxContainer/SpinBox").Value;
        map.Height = (int)this.GetNode<SpinBox>("VBoxContainer/HBoxContainer/SpinBox2").Value;
        map.AiCivNumber = (int)this.GetNode<SpinBox>("VBoxContainer/HBoxContainer2/SpinBox").Value;
        map.PlayerColor = this.GetNode<ColorPickerButton>("VBoxContainer/HBoxContainer3/ColorPickerButton").Color;

        this.QueueFree();
        this.GetTree().Root.AddChild(game);
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
