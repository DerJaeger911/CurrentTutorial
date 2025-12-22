using Godot;
using System.Collections.Generic;

namespace RoguelikeCourse.Scripts.Manager.PreloadManagers.ItemPreloads
{
	internal class ItemPreloads
	{
		private static ItemPreloads instance = new();
		public static ItemPreloads Instance => instance;

		public Dictionary<ItemEnum, PackedScene> Items { get; private set; } = [];

		private ItemPreloads()
		{
			this.Items[ItemEnum.HealthPotion] = ResourceLoader.Load<PackedScene>("res://Scenes/Items/UsedItems/item_health.tscn");
			this.Items[ItemEnum.DamagePotion] = ResourceLoader.Load<PackedScene>("res://Scenes/Items/UsedItems/item_damage.tscn");
			this.Items[ItemEnum.MoveSpeedPotion] = ResourceLoader.Load<PackedScene>("res://Scenes/Items/UsedItems/item_move_speed.tscn");
			this.Items[ItemEnum.ShootRatePotion] = ResourceLoader.Load<PackedScene>("res://Scenes/Items/UsedItems/item_shoot_rate.tscn");
		}

		public Item InstantiateItem(ItemEnum key)
		{
			return this.Items[key].Instantiate() as Item;
		}

		public Item GetRandomItem(params ItemEnum[] keys)
		{
			GD.Print(keys);
			if (keys.Length == 0)
			{
				return null;
			}

			var rnd = new RandomNumberGenerator();
			rnd.Randomize();

			ItemEnum key = keys[rnd.RandiRange(0, keys.Length - 1)];

			return this.InstantiateItem(key);
		}
	}
}
