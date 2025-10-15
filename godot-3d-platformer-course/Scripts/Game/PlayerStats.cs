using Godot;
using System;

public partial class PlayerStats : Node
{
    private int score = 0;

    public static PlayerStats Instance { get; private set; }

    public int Score { get => score; set => score = value; }

    public override void _Ready()
    {

        Instance = this;
    }
}
