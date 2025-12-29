using Godot;
using Godot.Collections;
using RoguelikeCourse.Scripts.Enums;
using RoguelikeCourse.Scripts.Manager.PreloadManagers.AssetPreloads;
using RoguelikeCourse.Scripts.Manager.PreloadManagers.LevelPreloads;
using System.Linq;

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

	private int firstRoomX = 3;
	private int firstRoomY = 3;
	private Room firstRoom;

	[Export]
	CharacterBody2D player;

    public int MapSize { get => this.mapSize; set => this.mapSize = value; }

    public override void _Ready()
	{
		this.AddPlayer();

		this.Generate();

		// Optional: print map for debugging
		foreach (var x in GD.Range(this.MapSize))
		{
			string line = string.Empty;
			foreach (var y in GD.Range(this.MapSize))
			{
				line += this.GetMap(x, y) ? "# " : "0 ";
			}
			GD.Print(line);
		}
	}

	private void AddPlayer()
	{
		if (this.player == null)
		{
			this.player = AssetPreloads.Player;
			this.GetTree().Root.GetNode("/root/Main").CallDeferred("add_child", this.player);
		}
	}

	private void Generate()
	{
		this.roomCount = 0;
		this.map.Resize(this.MapSize * this.MapSize);

		this.CheckRoom(this.firstRoomX, this.firstRoomY, Vector2.Zero, true);
		this.InstantiateRooms();
	}

	private void CheckRoom(int x, int y, Vector2 desiredDirection, bool isFirstRoom = false)
	{
		if (this.roomCount >= this.roomsToGenerate
			|| x < 0 || x >= this.MapSize || y < 0 || y >= this.MapSize
			|| this.GetMap(x, y))
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
		Vector2 bossRoomPosition = this.DecideBossRoom();

		// Instantiate all rooms
		foreach (var x in GD.Range(this.MapSize))
		{
			foreach (var y in GD.Range(this.MapSize))
			{
				if (!this.GetMap(x, y))
				{
					continue;
				}

				Room room;
				bool isFirstRoom = x == this.firstRoomX && y == this.firstRoomY;

				if (isFirstRoom)
				{
					room = LevelPreloads.Instance.InstantiateRoom(LevelEnum.Empty);
				}
				else if (x == bossRoomPosition.X && y == bossRoomPosition.Y)
				{
					room = LevelPreloads.Instance.InstantiateRoom(LevelEnum.Boss);
				}
				else
				{
					room = LevelPreloads.Instance.GetRandomRoom(LevelSets.InnerRooms.ToArray());
				}

				this.GetTree().Root.GetNode("/root/Main").CallDeferred(Node.MethodName.AddChild, room);
				this.rooms.Add(room);
				room.GlobalPosition = new Vector2(x, y) * this.roomPositionOffset;

				room.Initialize();

				if (isFirstRoom)
				{
					this.firstRoom = room;
				}
			}
		}

		// Set neighbors deferred
		foreach (Room room in this.rooms)
		{
			Vector2I mapPos = this.GetMapIndex(room);
			int x = mapPos.X;
			int y = mapPos.Y;

			if (y > 0 && this.GetMap(x, y - 1))
			{
				room.CallDeferred(nameof(Room.SetNeighbor), (int)DirectionEnums.North, this.GetRoomFromMap(x, y - 1));
			}

			if (y < this.MapSize - 1 && this.GetMap(x, y + 1))
			{
				room.CallDeferred(nameof(Room.SetNeighbor), (int)DirectionEnums.South, this.GetRoomFromMap(x, y + 1));
			}

			if (x < this.MapSize - 1 && this.GetMap(x + 1, y))
			{
				room.CallDeferred(nameof(Room.SetNeighbor), (int)DirectionEnums.East, this.GetRoomFromMap(x + 1, y));
			}

			if (x > 0 && this.GetMap(x - 1, y))
			{
				room.CallDeferred(nameof(Room.SetNeighbor), (int)DirectionEnums.West, this.GetRoomFromMap(x - 1, y));
			}
		}

		this.firstRoom.CallDeferred(nameof(Room.PlayerEnter), (int)DirectionEnums.North, this.player, true);
	}

	public Room GetRoomFromMap(int x, int y)
	{
		foreach (Room room in this.rooms)
		{
			Vector2I pos = this.GetMapIndex(room);
			if (pos.X == x && pos.Y == y)
			{
				return room;
			}
		}
		return null;
	}

	private Vector2 DecideBossRoom()
	{
		int i = 0;

		while (i < 100)
		{
			i++;

			int x = GD.RandRange(0, this.mapSize - 1);
			int y = GD.RandRange(0, this.mapSize - 1);
			GD.Print($"I is {i}");
			GD.Print($"FX={this.firstRoomX}, FY={this.firstRoomY}, X={x}, Y={y}");
			if (this.firstRoomX == x && this.firstRoomY == y)
			{
				continue;
			}

			if (this.GetMap(x, y))
			{
				return new Vector2(x, y);
			}
		}


		return new Vector2(0, 0);
	}


	private Vector2I GetMapIndex(Room room)
	{
		Vector2 pos = room.GlobalPosition / this.roomPositionOffset;
		return new Vector2I((int)pos.X, (int)pos.Y);
	}

	private bool GetMap(int x, int y) => this.map[x + y * this.MapSize];
	private void SetMap(int x, int y, bool value) => this.map[x + y * this.MapSize] = value;
}
