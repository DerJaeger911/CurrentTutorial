using Godot;
using Godot.Collections;
using System;

namespace RoguelikeCourse.Scripts;

public partial class RoomGeneration : Node
{
    [Export]
    private int mapSize = 7;
    [Export]
    private int roomsToGenerate = 12;

    private int roomCount = 0;
    private Array<bool> map = [];
    private Array<Room> rooms = [];
    private float roomPositionOffset = 160;

    private PackedScene roomScene = GD.Load("res://Scenes/Rooms/room_template.tscn") as PackedScene;

    private int firstRoomX = 3;
    private int firstRoomY = 3;
    private Room firstRoom;

    [Export]
    CharacterBody2D player;

    public override void _Ready()
    {
        this.Generate();

        foreach (var x in GD.Range(this.mapSize))
        {
            string line = string.Empty;

            foreach (var y in GD.Range(this.mapSize))
            {
                line += this.GetMap(x, y) ? "# " : "0 ";
            }
            GD.Print(line);
        }
    }

    private void Generate()
    {
        this.roomCount = 0;
        this.map.Resize(this.mapSize * this.mapSize);

        this.CheckRoom(this.firstRoomX, this.firstRoomY, Vector2.Zero, true);
        this.InstantiateRooms();
    }

    private void CheckRoom(int x, int y, Vector2 desiredDirection, bool isFirstRoom = false)
    {
        if (this.roomCount >= this.roomsToGenerate
            || x < 0 || x > this.mapSize - 1 || y < 0 || y > this.mapSize - 1
            || this.GetMap(x, y) == true)
        {
            return;
        }

        this.roomCount++;
        this.SetMap(x, y, true);

        bool goNorth = GD.Randf() > (desiredDirection == Vector2.Up ? 0.2f : 0.8f);
        bool goEast = GD.Randf() > (desiredDirection == Vector2.Right ? 0.2f : 0.8f);
        bool goWest = GD.Randf() > (desiredDirection == Vector2.Left ? 0.2f : 0.8f);
        bool goSouth = GD.Randf() > (desiredDirection == Vector2.Down ? 0.2f : 0.8f);

        if (goNorth || isFirstRoom)
        {
            this.CheckRoom(x, y - 1, isFirstRoom ? Vector2.Up : desiredDirection);
        }
        if (goEast || isFirstRoom)
        {
            this.CheckRoom(x + 1, y, isFirstRoom ? Vector2.Right : desiredDirection);
        }
        if (goWest || isFirstRoom)
        {
            this.CheckRoom(x - 1, y, isFirstRoom ? Vector2.Left : desiredDirection);
        }
        if (goSouth || isFirstRoom)
        {
            this.CheckRoom(x, y + 1, isFirstRoom ? Vector2.Down : desiredDirection);
        }
    }

    private void InstantiateRooms()
    {
        foreach (var x in GD.Range(this.mapSize))
        {
            foreach (var y in GD.Range(this.mapSize))
            {
                if (this.GetMap(x, y) == false)
                {
                    continue;
                }

                Room room = this.roomScene.Instantiate() as Room;
                this.GetTree().Root.CallDeferred(Node.MethodName.AddChild, room);
                this.rooms.Add(room);
                room.GlobalPosition = new Vector2(x, y) * this.roomPositionOffset;
            }
        }
    }

    private bool GetMap(int x, int y)
    {
        return this.map[x + y * this.mapSize];
    }

    private void SetMap(int x, int y, bool value)
    {
        this.map[x + y * this.mapSize] = value;
    }
}
