using FarmingRpg.Scripts.Enums;
using Godot;
using System;

namespace FarmingRpg.Scripts.UI;

public partial class ToolButton : TextureButton
{
    [Export]
    private ToolEnums tool;
    [Export]
    private CropData seed;

    private Label quantityLabel;

    public override void _Ready()
    {
        this.quantityLabel = this.GetNode<Label>("QuantityText");
        this.quantityLabel.Text = string.Empty;

        this.Pressed += this.OnPressed;
        this.MouseEntered += this.OnMouseEntered;
        this.MouseExited += this.OnMouseExited;

        this.PivotOffset = this.Size / 2;
    }

    private void OnPressed()
    {

    }

    private void OnMouseEntered()
    {
        this.Scale = new Vector2(1.05f, 1.05f);
    }

    private void OnMouseExited()
    {
        this.Scale = new Vector2(1, 1);
    }

    private void OnChangeSeedQuantity(CropData cropData, int quantity)
    {
        if (this.seed != cropData)
        {
            return;
        }

        this.quantityLabel.Text = quantity.ToString();
    }

    public override void _ExitTree()
    {
        this.Pressed -= this.OnPressed;
        this.MouseEntered -= this.OnMouseEntered;
        this.MouseExited -= this.OnMouseExited;
    }
}
