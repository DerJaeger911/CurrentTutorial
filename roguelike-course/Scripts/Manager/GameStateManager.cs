using Godot;
using RoguelikeCourse.Scripts.Manager.Signals;

namespace RoguelikeCourse.Scripts.Manager;

internal class GameStateManager
{
    private static GameStateManager instance = new();
    public static GameStateManager Instance => instance;

    public bool IsInMenu { get; private set; }

    private GameStateManager()
    {
        GD.Print("GameManager is Init");
        GameSignals.Instance.CallDeferred(nameof(GameSignals.EmitInMenuState), this.IsInMenu);
    }

    public void EnterMenu()
    {
        this.IsInMenu = true;
        GameSignals.Instance.EmitSignal(nameof(GameSignals.InMenuState), this.IsInMenu);
    }
    public void ExitMenu()
    {
        this.IsInMenu = false;
        GameSignals.Instance.EmitSignal(nameof(GameSignals.InMenuState), this.IsInMenu);
    }
}
