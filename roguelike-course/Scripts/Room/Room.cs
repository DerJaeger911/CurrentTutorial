using Godot;
using RoguelikeCourse.Scripts.Enums;
using RoguelikeCourse.Scripts.Manager.Signals;
using System;

namespace RoguelikeCourse.Scripts;

public partial class Room : StaticBody2D
{
    [Export]
    private bool doorsAlwaysOpen = false;

    private int enemiesInRoom;

    private RoomEntrance roomEntranceNorth;
    private RoomEntrance roomEntranceSouth;
    private RoomEntrance roomEntranceWest;
    private RoomEntrance roomEntranceEast;

    public override void _Ready()
    {
        GameSignals.Instance.EnemyDefeated += this.OnEnemyDefeated;
        this.roomEntranceNorth = this.GetNode<RoomEntrance>("EntranceNorth");
        this.roomEntranceSouth = this.GetNode<RoomEntrance>("EntranceSouth");
        this.roomEntranceWest = this.GetNode<RoomEntrance>("EntranceWest");
        this.roomEntranceEast = this.GetNode<RoomEntrance>("EntranceEast");

        foreach (Node child in this.GetChildren())
        {
            if (child is Enemy enemy)
            {
                this.enemiesInRoom += 1;
                enemy.Initialize(this);
            }
        }
    }

    public void Initialize()
    {

    }

    public void SetNeighbor(int neighborDirectionAsInt, Room neighborRoom)
    {
        DirectionEnums neighborDirection = (DirectionEnums)neighborDirectionAsInt;

        RoomEntrance entrance = neighborDirection switch
        {
            DirectionEnums.North => this.roomEntranceNorth,
            DirectionEnums.South => this.roomEntranceSouth,
            DirectionEnums.West => this.roomEntranceWest,
            DirectionEnums.East => this.roomEntranceEast,
            _ => throw new ArgumentOutOfRangeException(nameof(neighborDirectionAsInt), neighborDirection, "Invalid direction")
        };

        entrance.SetNeighbor(neighborRoom);
    }

    public void PlayerEnter(int entryDirectionAsInt, CharacterBody2D player, bool firstRoom = false)
    {
        DirectionEnums entryDirection = (DirectionEnums)entryDirectionAsInt;
        //if firstRoom = true -> Keep this.
        Vector2 currentSpawn = this.GlobalPosition;

        if (firstRoom)
        {
            currentSpawn = this.GlobalPosition;
        }
        else if (!firstRoom) 
        {
            currentSpawn = entryDirection switch
            {
                DirectionEnums.North => this.roomEntranceNorth.PlayerSpawn.GlobalPosition,
                DirectionEnums.South => this.roomEntranceSouth.PlayerSpawn.GlobalPosition,
                DirectionEnums.West => this.roomEntranceWest.PlayerSpawn.GlobalPosition,
                DirectionEnums.East => this.roomEntranceEast.PlayerSpawn.GlobalPosition,
                _ => player.GlobalPosition
            };
        }
        player.GlobalPosition = currentSpawn;

        GameSignals.Instance.EmitSignal(nameof(GameSignals.PlayerEnteredRoom), this);

        if (this.enemiesInRoom > 0 && !this.doorsAlwaysOpen)
        {
            this.CloseDoors();
        }
        else
        {
            this.OpenDoors();
        }
    }

    private void OpenDoors()
    {
        this.roomEntranceNorth.CallDeferred("OpenDoor");
        this.roomEntranceSouth.CallDeferred("OpenDoor");
        this.roomEntranceWest.CallDeferred("OpenDoor");
        this.roomEntranceEast.CallDeferred("OpenDoor");
    }

    private void CloseDoors()
    {
        this.roomEntranceNorth.CallDeferred("CloseDoor");
        this.roomEntranceSouth.CallDeferred("CloseDoor");
        this.roomEntranceWest.CallDeferred("CloseDoor");
        this.roomEntranceEast.CallDeferred("CloseDoor");
    }

    private void OnEnemyDefeated(Enemy enemy)
    {
        if(enemy.GetParent() == this)
        {
            this.enemiesInRoom -= 1;
        }

        if(this.enemiesInRoom <= 0)
        {
            this.OpenDoors();
        }
    }

    public override void _ExitTree()
    {
        GameSignals.Instance.EnemyDefeated -= this.OnEnemyDefeated;
    }
}