using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Enums;
using Godot;
using Godot.Collections;

namespace FarmingRpg.Scripts.Manager.AutoLoad;

public partial class GameManager : Node
{
    private int day = 0;
    private int money = 0;

    private Array<CropData> allCropData =
    [
        GD.Load<CropData>("res://Resources/Crops/corn.tres"),
        GD.Load<CropData>("res://Resources/Crops/tomato.tres"),
    ];

    private Dictionary<CropData, int> ownedSeeds = [];

    public static GameManager Instance { get; private set; }
    public Dictionary<CropData, int> OwnedSeeds { get => this.ownedSeeds; set => this.ownedSeeds = value; }

    public override void _Ready()
    {
        this.GetTree().SceneChanged += this.OnSceneChanged;

            if (Instance != null)
        {
            this.QueueFree();
            return;
        }
        Instance = this;


        if (this.GetTree().CurrentScene.Name == "Main")
        {
            this.OnSceneChanged();
        }


    }

    private void OnSceneChanged()
    {
        if(this.GetTree().CurrentScene.Name != "Main")
        {
            return;
        }
        GD.Print("Isse good");

        this.CallDeferred(nameof(this.DeferredInit));
        this.CallDeferred(nameof(this.GiveMoney), 10);
        this.CallDeferred(nameof(this.SetNextDay));

    }

    private void DeferredInit()
    {
        GameSignals.Instance.Connect(SignalConsts.ConsumeSeed, new Callable(this, nameof(ConsumeSeed)));
        foreach (CropData cd in this.allCropData)
        {
            this.GiveSeed( cd, 2);
        }
    }

    public void SetNextDay()
    {
        this.day += 1;
        GameSignals.Instance.EmitSignal(SignalConsts.NewDay, this.day);
    }

    public void HarvestCrop(Crop crop)
    {
        this.GiveMoney(crop.CropData.SellPrice);
        GameSignals.Instance.EmitSignal(SignalConsts.HarvestCrop, crop);
        crop.QueueFree();
    }

    public void TryBuySeed(CropData cropData)
    {
        if(this.money < cropData.SeedPrice)
        {
            return;
        }

        this.money -= cropData.SeedPrice;
        this.OwnedSeeds[cropData] += 1;
        GameSignals.Instance.EmitSignal(SignalConsts.ChangeMoney, this.money);
        GameSignals.Instance.EmitSignal(SignalConsts.ChangeSeed, cropData, this.OwnedSeeds[cropData]);
    }

    private void ConsumeSeed(CropData cropData)
    {
        this.OwnedSeeds[cropData] -= 1;
        GameSignals.Instance.EmitSignal(SignalConsts.ChangeSeed, cropData, this.OwnedSeeds[cropData]);
    }

    private void GiveMoney(int amount)
    {
        this.money += amount;
        GameSignals.Instance.EmitSignal(SignalConsts.ChangeMoney, this.money);
    }

    private void GiveSeed(CropData cropData, int amount)
    {
        if (this.OwnedSeeds.TryGetValue(cropData, out int current))
        {
            this.OwnedSeeds[cropData] = current + amount;
        }
        else
        {
            this.OwnedSeeds[cropData] = amount;
        }
        GameSignals.Instance.EmitSignal(SignalConsts.ChangeSeed, cropData, this.OwnedSeeds[cropData]);
    }

    public override void _ExitTree()
    {
        GameSignals.Instance.Disconnect(SignalConsts.ConsumeSeed, new Callable(this, nameof(ConsumeSeed)));
    }
}
