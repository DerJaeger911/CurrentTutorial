using Godot;
using Godot.Collections;

namespace FarmingRpg.Scripts;

[GlobalClass]
public partial class CropData : Resource
{
    [Export]
    private Array<Texture2D> growthSprites = [];
    [Export]
    private int daysToGrow = 8;
    [Export]
    private int seedPrice = 10;
    [Export]
    private int sellPrice = 20;

    public Array<Texture2D> GrowthSprites { get => this.growthSprites; set => this.growthSprites = value; }
    public int DaysToGrow { get => this.daysToGrow; set => this.daysToGrow = value; }
    public int SeedPrice { get => this.seedPrice; set => this.seedPrice = value; }
    public int SellPrice { get => this.sellPrice; set => this.sellPrice = value; }
}
