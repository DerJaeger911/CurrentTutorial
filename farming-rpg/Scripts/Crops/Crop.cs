using Godot;
using System;

namespace FarmingRpg.Scripts;

public partial class Crop : Node2D
{
    private CropData cropData;
    private int daysUntilGrown;
    private bool watered;
    private bool harvestable;
    private Vector2I tileMapCoords;
    private Sprite2D cropSprite;

    public bool Harvestable { get => this.harvestable; set => this.harvestable = value; }
    public bool Watered { get => this.watered; set => this.watered = value; }

    public override void _Ready()
    {
        this.cropSprite = this.GetNode<Sprite2D>("Sprite");
    }

    public void SetCrop(CropData data, bool isWatered, Vector2I tileCoords)
    {
        this.cropData = data;
        this.Watered = isWatered;
        this.tileMapCoords = tileCoords;
        this.Harvestable = false;

        this.daysUntilGrown = data.DaysToGrow;

        this.cropSprite.Texture = this.cropData.GrowthSprite[0];
    }

    private void OnNewDay(int day)
    {

    }
}
