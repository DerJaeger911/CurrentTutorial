using Godot;

namespace bullethellcourse.Scripts.Managers;

internal class DummyManager
{
    private static DummyManager instance = new();
    public static DummyManager Instance => instance;

    // This is ready now so we take the Constructor
    private DummyManager()
    {
        GD.Print("DummyManager is Init");
    }
}
