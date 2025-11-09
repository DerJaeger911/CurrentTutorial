using FarmingRpg.Scripts;
using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Enums;
using FarmingRpg.Scripts.Manager;
using FarmingRpg.Scripts.Manager.AutoLoad;
using Godot;

namespace FarmingRpg.Scripts.Player;

public partial class PlayerTools : Node2D
{
    private ToolEnums currentTool;
    private CropData currentSeed;

    private FarmManager farmManager;

    public ToolEnums CurrentTool { get => this.currentTool; set => this.currentTool = value; }
    public CropData CurrentSeed { get => this.currentSeed; set => this.currentSeed = value; }

    public override void _Ready()
    {
        GameSignals.Instance.Connect(SignalConsts.PlayerTool, new Callable(this, nameof(SetTool)));
        this.CurrentTool = ToolEnums.Hoe;

        this.CallDeferred(nameof(this.DeferredInit));

        this.farmManager = this.GetNode<FarmManager>("../../FarmManager");
    }

    private void DeferredInit()
    {
        GameSignals.Instance.EmitSignal(SignalConsts.PlayerTool, (int)this.CurrentTool, this.CurrentSeed);
    }

    private void SetTool(ToolEnums tool, CropData seed)
    {
        this.CurrentTool = tool;
        this.CurrentSeed = seed;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed(PlayerInputConsts.Ineract))
        {
            switch (this.CurrentTool)
            {
                case ToolEnums.Hoe:
                    this.farmManager.TryTillTile(this.GlobalPosition);
                    break;
                case ToolEnums.WaterBucket:
                    this.farmManager.TryWaterTile(this.GlobalPosition);
                    break;
                case ToolEnums.Scythe:
                    this.farmManager.TryHarvestTile(this.GlobalPosition);
                    break;
                case ToolEnums.Seed:
                    this.farmManager.TrySeedTile(this.GlobalPosition, this.CurrentSeed);
                    break;
            }
        }
    }
}
