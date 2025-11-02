using FarmingRpg.Scripts;
using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Enums;
using Godot;

public partial class PlayerTools : Node2D
{
    private ToolEnums currentTool;
    private CropData currentSeed;

    private FarmManager farmManager;

    public override void _Ready()
    {
        this.farmManager = this.GetNode<FarmManager>("../../FarmManager");

        this.currentTool = ToolEnums.Hoe;
    }

    private void SetTool(ToolEnums tool, CropData seed)
    {
        this.currentTool = tool;
        this.currentSeed = seed;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed(PlayerInputConsts.Ineract))
        {
            switch (this.currentTool)
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
                    this.farmManager.TrySeedTile(this.GlobalPosition, this.currentSeed);
                    break;
            }
        }
    }
}
