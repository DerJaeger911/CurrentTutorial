using Godot;

namespace RoguelikeCourse.Scripts;

public partial class RoomEntrance : Node2D
{
    [Export]
    private DirectionEnums direction = DirectionEnums.North;

    private StaticBody2D barrier;
    private CollisionShape2D barrierCollider;

    private Node2D door;
    private StaticBody2D doorShut;
    private CollisionShape2D doorShutCollider;

    private Node2D playerSpawn;
    private Area2D exitTrigger;

    private Room neighbor;

    public override void _Ready()
    {
        this.barrier = this.GetNode<StaticBody2D>("Barrier");
        this.barrierCollider = this.GetNode<CollisionShape2D>("Barrier/CollisionShape2D");

        this.door = this.GetNode<Node2D>("Door");
        this.doorShut = this.GetNode<StaticBody2D>("Door/DoorShut");
        this.doorShutCollider = this.GetNode<CollisionShape2D>("Door/DoorShut/CollisionShape2D");

        this.playerSpawn = this.GetNode<Node2D>("PlayerSpawn");
        this.exitTrigger = this.GetNode<Area2D>("ExxitTrigger");

        this.exitTrigger.BodyEntered += this.OnBodyEnteredExitTrigger;
    }

    private void SetNeighbor(Room neighborRoom)
    {
        this.neighbor = neighborRoom;
        this.ToggleBarrier(false);
    }

    private void ToggleBarrier(bool toggle)
    {
        this.barrier.Visible = toggle;
        this.barrierCollider.Disabled = !toggle;

        this.door.Visible = !toggle;
    }

    private void OpenDoor()
    {
        if (this.barrier.Visible)
        {
            return;
        }

        this.doorShut.Visible = false;
        this.doorShutCollider.Disabled = true;
    }

    private void CloseDoor()
    {
        if (this.barrier.Visible)
        {
            return;
        }

        this.doorShut.Visible = true;
        this.doorShutCollider.Disabled = false;
    }

    private void OnBodyEnteredExitTrigger(Node body) 
    { 
        
    }

    private DirectionEnums GetNeighborEntryDirection()
    {
        if(this.direction == DirectionEnums.North)
        {
            return DirectionEnums.South;
        }
        else if (this.direction == DirectionEnums.South)
        {
            return DirectionEnums.North;
        }
        else if (this.direction == DirectionEnums.West)
        {
            return DirectionEnums.East;
        }
        else
        {
            return DirectionEnums.South;
        }
    }
}
