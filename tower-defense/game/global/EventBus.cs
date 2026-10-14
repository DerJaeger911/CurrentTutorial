using Godot;
using System;

public partial class EventBus : Node
{
    // Hier definierst du deine Events (z. B. wenn eine Tower-Karte geklickt wird)
    [Signal] public delegate void PlaceTowerEventHandler(int towerId);

    // Eine statische Instanz, damit du von überall bequem darauf zugreifen kannst
    public static EventBus Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
}