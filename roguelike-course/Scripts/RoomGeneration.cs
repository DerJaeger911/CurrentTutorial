using Godot;
using Godot.Collections;

namespace RoguelikeCourse.Scripts;

public partial class RoomGeneration : Node
{
	[Export]
	private int mapSize = 7;
	[Export]
	private int roomsToGenerate = 12;

	private int roomCount = 0;
	private Array<bool> map;
	private Array<Room> rooms;
	private float roomPositionOffset = 160;

	private PackedScene roomScene = GD.Load("res://Scenes/Rooms/room_template.tscn") as PackedScene;

	private int firstRoomX = 3;
	private int firstRoomY = 3;
	private Room firstRoom;

	[Export]
	CharacterBody2D player;

	public override void _Ready()
	{
		this.Generate()
	}

	private void Generate()
	{
		this.roomCount = 0;
		this.map.Resize(this.mapSize * this.mapSize);

		this.CheckRoom(this.firstRoomX, this.firstRoomY, Vector2.Zero, true);
		this.InstantiateRooms()
	}

	private void CheckRoom(int x, int y, Vector2 desiredDirection, bool isFirstRoom = false)
	{

	}

	private void InstantiateRooms()
	{

	}
}
