using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Enums;
using FarmingRpg.Scripts.Manager.AutoLoad;
using Godot;
using System;
using System.Collections.Generic;

namespace FarmingRpg.Scripts.UI;

public partial class ToolButton : TextureButton
{
    [Export]
    private ToolEnums tool;
    [Export]
    private CropData seed;

    private Label quantityLabel;

    public ToolEnums Tool { get => this.tool; set => this.tool = value; }
    public CropData Seed { get => this.seed; set => this.seed = value; }

    public override void _Ready()
    {
        this.quantityLabel = this.GetNode<Label>("QuantityText");
        this.quantityLabel.Text = string.Empty;

        this.Pressed += this.OnPressed;
        this.MouseEntered += this.OnMouseEntered;
        this.MouseExited += this.OnMouseExited;

        this.PivotOffset = this.Size / 2;

        this.CallDeferred(nameof(DeferredConnect));
    }

    private void DeferredConnect()
    {
        GameSignals.Instance.Connect(SignalConsts.ChangeSeed, new Callable(this, nameof(OnChangeSeedQuantity)));
        if (GameManager.Instance != null && this.Seed != null)
        {
            foreach (KeyValuePair<CropData, int> kvp in GameManager.Instance.OwnedSeeds)
            {
                this.OnChangeSeedQuantity(kvp.Key, kvp.Value);
            }
        }
    }

    private void OnPressed()
    {
        GameSignals.Instance.EmitSignal(SignalConsts.PlayerTool, (int)this.Tool, this.Seed);
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

        if (this.Seed == null || this.Seed.GetInstanceId() != cropData.GetInstanceId())
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
        GameSignals.Instance.Disconnect(SignalConsts.ChangeSeed, new Callable(this, nameof(OnChangeSeedQuantity)));
    }
}
