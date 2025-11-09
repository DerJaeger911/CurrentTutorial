using FarmingRpg.Scripts;
using FarmingRpg.Scripts.Manager.AutoLoad;
using Godot;
using System;

namespace FarmingRpg.Scripts.UI;

[GlobalClass]
public partial class BuySeedButton : BaseButton
{
    [Export]
    private CropData cropData;

    private Label priceText;
    private TextureRect icon;

    public override void _Ready()
    {
        this.priceText = this.GetNode<Label>("PriceText");
        this.icon = this.GetNode<TextureRect>("Icon");

        this.Pressed += this.OnPress;

        if (this.cropData == null)
        {
            return;
        }

        if (this.cropData.GrowthSprites.Count > 0)
        {
            this.icon.Texture = this.cropData.GrowthSprites[^1];
        }
        else
        {
            GD.PrintErr($"CropData {this.cropData.ResourceName} has NO growth sprites assigned!");
        }

        this.priceText.Text = $"$ {this.cropData.SeedPrice}";
        this.icon.Texture = this.cropData.GrowthSprites[^1];
    }

    private void OnPress()
    {
        GameManager.Instance.TryBuySeed(this.cropData);
    }

    public override void _ExitTree()
    {
        this.Pressed -= this.OnPress;
    }
}
