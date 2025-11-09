using FarmingRpg.Scripts;
using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Enums;
using FarmingRpg.Scripts.Manager.AutoLoad;
using FarmingRpg.Scripts.UI;
using Godot;
using Godot.Collections;
using System.Linq;

namespace FarmingRpg.Scripts.Manager;

public partial class UiManager : CanvasLayer
{
    private Array<ToolButton> toolButtons = [];

    private Label moneyLabel;
    private Label dayLabel;
    private Button nextDayButton;

    public override void _Ready()
    {
        this.moneyLabel = this.GetNode<Label>("MoneyText");
        this.dayLabel = this.GetNode<Label>("DayText");
        this.nextDayButton = this.GetNode<Button>("NextDayButton");

        this.nextDayButton.Pressed += this.OnNextDayButtonPressed;

        foreach (ToolButton button in this.GetNode<HBoxContainer>("ToolButtons").GetChildren().OfType<ToolButton>())
        {
            this.toolButtons.Add(button);
            GD.Print(this.toolButtons);
        }
        GameSignals.Instance.Connect(SignalConsts.PlayerTool, new Callable(this, nameof(OnSetTool)));
        GameSignals.Instance.Connect(SignalConsts.ChangeMoney, new Callable(this, nameof(OnMoneyChanged)));
        GameSignals.Instance.Connect(SignalConsts.NewDay, new Callable(this, nameof(OnDayChanged)));
    }

    private void OnSetTool(int tools, CropData seed) 
    {
        ToolEnums playerTools = (ToolEnums)tools;

        foreach (ToolButton button in this.toolButtons)
        {
            if (button == null)
                continue;

            bool isActive;

            if (seed == null)
            {
                isActive = button.Tool == playerTools;
            }
            else
            {
                isActive = button.Tool == playerTools && button.Seed.GetInstanceId() == seed.GetInstanceId();
            }

            button.SelfModulate = isActive ? Colors.Green : Colors.White;
        }
    }

    private void OnDayChanged(int day)
    {
        this.dayLabel.Text = $"Day {day}";
    }

    private void OnMoneyChanged(int money)
    {
        this.moneyLabel.Text = $"${money}";
    }

    private void OnNextDayButtonPressed()
    {
        GameManager.Instance.SetNextDay();
    }

    public override void _ExitTree()
    {
        this.nextDayButton.Pressed -= this.OnNextDayButtonPressed;
        GameSignals.Instance.Disconnect(SignalConsts.PlayerTool, new Callable(this, nameof(OnSetTool)));
        GameSignals.Instance.Disconnect(SignalConsts.ChangeMoney, new Callable(this, nameof(OnMoneyChanged)));
        GameSignals.Instance.Disconnect(SignalConsts.NewDay, new Callable(this, nameof(OnDayChanged)));
    }
}
