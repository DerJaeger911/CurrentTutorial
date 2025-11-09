using FarmingRpg.Scripts.Enums;
using Godot;

namespace FarmingRpg.Scripts.Manager.AutoLoad
{
    internal partial class GameSignals : Node
    {
        [Signal]
        public delegate void NewDayEventHandler(int day);

        [Signal]
        public delegate void PlayerToolEventHandler(int tool, CropData seed);

        [Signal]
        public delegate void HarvestCropEventHandler(Crop crop);

        [Signal]
        public delegate void ChangeSeedQuantityEventHandler(CropData cropData, int quantity);

        [Signal]
        public delegate void ChangeMoneyEventHandler(int money);

        [Signal]
        public delegate void ConsumeSeedEventHandler(CropData cropData);

        public static GameSignals Instance { get; private set; }

        public override void _Ready()
        {
            if (Instance != null)
            {
                GD.PrintErr("GameSignals already exists!");
                this.QueueFree();
                return;
            }
            GD.Print("Signals initialized");
            Instance = this;
        }
    }
}
