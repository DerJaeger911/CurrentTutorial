using Godot;
using System.Collections.Generic;

namespace RoguelikeCourse.Scripts.Manager.PreloadManagers.LevelPreloads
{
    public partial class LevelPreloads
    {
        private static LevelPreloads instance = new();
        public static LevelPreloads Instance => instance;

        public Dictionary<LevelEnum, PackedScene> Rooms { get; private set; } = [];

        private LevelPreloads()
        {
            this.Rooms[LevelEnum.Empty] = ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/UsedRooms/room_empty.tscn");
            this.Rooms[LevelEnum.Enemy1] = ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/UsedRooms/room_enemy1.tscn");
            this.Rooms[LevelEnum.Enemy2] = ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/UsedRooms/room_enemy2.tscn");
            this.Rooms[LevelEnum.Enemy3] = ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/UsedRooms/room_enemy3.tscn");
            this.Rooms[LevelEnum.Enemy4] = ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/UsedRooms/room_enemy4.tscn");
            this.Rooms[LevelEnum.Item] = ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/UsedRooms/room_item.tscn");
            this.Rooms[LevelEnum.Boss] = ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/UsedRooms/room_boss.tscn");
		}

        public Room InstantiateRoom(LevelEnum key)
        {
            return this.Rooms[key].Instantiate() as Room;
        }

        public Room GetRandomRoom(params LevelEnum[] keys)
        {
            if (keys.Length == 0)
            {
                return null;
            }

            var rnd = new RandomNumberGenerator();
            rnd.Randomize();

            LevelEnum key = keys[rnd.RandiRange(0, keys.Length - 1)];

            return this.InstantiateRoom(key);
        }
    }
}
