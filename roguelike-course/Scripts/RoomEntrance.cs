using Godot;
using RoguelikeCourse.Scripts.Interfaces;

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

    public Node2D PlayerSpawn { get => this.playerSpawn; set => this.playerSpawn = value; }

    public override void _Ready()
    {
        this.barrier = this.GetNode<StaticBody2D>("Barrier");
        this.barrierCollider = this.GetNode<CollisionShape2D>("Barrier/CollisionShape2D");

        this.door = this.GetNode<Node2D>("Door");
        this.doorShut = this.GetNode<StaticBody2D>("Door/DoorShut");
        this.doorShutCollider = this.GetNode<CollisionShape2D>("Door/DoorShut/CollisionShape2D");

        this.PlayerSpawn = this.GetNode<Node2D>("PlayerSpawn");
        this.exitTrigger = this.GetNode<Area2D>("ExitTrigger");

        this.exitTrigger.BodyEntered += this.OnBodyEnteredExitTrigger;

        this.ToggleBarrier(true);
    }

    public void SetNeighbor(Room neighborRoom)
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

    public void OpenDoor()
    {
        if (this.barrier.Visible)
        {
            return;
        }

        this.doorShut.Visible = false;
        this.doorShutCollider.Disabled = true;
    }

    public void CloseDoor()
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
        if(body is IPlayer && body is CharacterBody2D playerBody)
        {
            this.neighbor.PlayerEnter((int)this.GetNeighborEntryDirection(), playerBody);
        }
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
