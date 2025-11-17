using Godot;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Manager.Signals;

public partial class CameraController : Camera2D
{

    public override void _Ready()
    {
        GameSignals.Instance.PlayerEnteredRoom += this.OnPlayerEnteredRoom;
    }


    private void OnPlayerEnteredRoom(Room room)
    {
        this.GlobalPosition = room.GlobalPosition;
    }

    public override void _ExitTree()
    {
        GameSignals.Instance.PlayerEnteredRoom -= this.OnPlayerEnteredRoom;
    }
}
