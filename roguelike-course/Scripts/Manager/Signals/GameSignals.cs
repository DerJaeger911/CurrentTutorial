using Godot;

namespace RoguelikeCourse.Scripts.Manager.Signals;

public partial class GameSignals : Node
{
    private static GameSignals instance = new();
    public static GameSignals Instance => instance;

    // This is ready now so we take the Constructor
    private GameSignals()
    {
        GD.Print("GameSignals is Init");
    }

    [Signal] 
    public delegate void PlayerEnteredRoomEventHandler(Room room);

    [Signal]
    public delegate void EnemyDefeatedEventHandler(Enemy enemy);

    [Signal]
    public delegate void InMenuStateEventHandler(bool isInMenu);

    public void EmitInMenuState(bool isInMenu)
    {
        this.EmitSignal(nameof(InMenuState), isInMenu);
    }
}
