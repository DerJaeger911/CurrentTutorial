using Godot;
using Godot3dPlatformerCourse.Scripts.Game.Player;
using System;
using System.Collections.Generic;

public partial class PlayerHud : CanvasLayer
{
    private HBoxContainer healthContainer;
    private List<TextureRect> hearts = new();

    public override void _Ready()
    {
        Player player = (Player)GetParent();

        

        healthContainer = GetNode<HBoxContainer>("HealthContainer");

        player.OnTakeDamage += UpdateHearts;
        player.OnScoreChanged += UpdateScoreText;
    }

    private void UpdateHearts(int hp)
    {
        
    }

    private void UpdateScoreText(int score)
    {

    }

    public override void _ExitTree()
    {
        player.OnTakeDamage += UpdateHearts;
        player.OnScoreChanged += UpdateScoreText;
    }
}
