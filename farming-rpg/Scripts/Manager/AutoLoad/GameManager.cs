using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Enums;
using Godot;
using Godot.Collections;

namespace FarmingRpg.Scripts.Manager.AutoLoad;

public partial class GameManager : Node
{
    private int day = 1;
    private int money = 0;

    private Array<CropData> allCropData = new Array<CropData>()
    {
        GD.Load<CropData>("res://Resources/Crops/corn.tres"),
        GD.Load<CropData>("res://Resources/Crops/tomato.tres"),
    };

    private Dictionary<CropData, int> ownedSeeds = [];

   

    public static GameManager Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PrintErr("GameManager already exists!");
            this.QueueFree();
            return;
        }

        Instance = this;
    }

    private void SetNextDay()
    {

    }

    private void HarvestCrop(Crop crop)
    {

    }

    private void TryBuySeed(CropData cropData)
    {

    }
    
    private void ConsumeSeed(CropData cropData)
    {
        this.ownedSeeds[cropData] -= 1; 
        GameSignals.Instance.EmitSignal(SignalConsts.ChangeSeed, this.ownedSeeds[cropData])
    }

    private void GiveMoney(int amount)
    {
        this.money += amount;
        GameSignals.Instance.EmitSignal(SignalConsts.ChangeMoney, this.money);
    }
}
