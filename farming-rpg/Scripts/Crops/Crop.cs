using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Manager.AutoLoad;
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
    public CropData CropData { get => this.cropData; set => this.cropData = value; }
    public Vector2I TileMapCoords { get => this.tileMapCoords; set => this.tileMapCoords = value; }

    public override void _Ready()
    {
        this.cropSprite = this.GetNode<Sprite2D>("Sprite");
        GameSignals.Instance.Connect(SignalConsts.NewDay, new Callable(this, nameof(OnNewDay)));
    }

    public void SetCrop(CropData data, bool isWatered, Vector2I tileCoords)
    {
        this.CropData = data;
        this.Watered = isWatered;
        this.TileMapCoords = tileCoords;
        this.Harvestable = false;

        this.daysUntilGrown = data.DaysToGrow;

        this.cropSprite.Texture = this.CropData.GrowthSprites[0];
    }

    private void OnNewDay(int day)
    {
        if (!this.Watered)
        {
            return;
        }
        this.Watered = false;

        if (this.daysUntilGrown != 0)
        {
            this.daysUntilGrown--;
        }
        else
        {
            this.harvestable = true;
        }

        int index = this.CropData.DaysToGrow - this.daysUntilGrown - 1;
        index = Math.Clamp(index, 0, this.CropData.GrowthSprites.Count - 1);

        this.cropSprite.Texture = this.CropData.GrowthSprites[index];

        GD.Print(index);
    }

    public override void _ExitTree()
    {
        GameSignals.Instance.Disconnect(SignalConsts.NewDay, new Callable(this, nameof(OnNewDay)));
    }
}
