using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Items;
using Godot;
using System.Collections.Generic;

namespace bullethellcourse.Scripts.Managers;

internal class ItemLoadManager
{
	private static ItemLoadManager instance = new();
	public static ItemLoadManager Instance => instance;

	private Dictionary<PotionTypeEnum, PackedScene> itemScenes = new();

	private ItemLoadManager()
	{
		this.itemScenes[PotionTypeEnum.Health] = GD.Load<PackedScene>("res://Scenes/health_potion.tscn");
		this.itemScenes[PotionTypeEnum.MoveSpeed] = GD.Load<PackedScene>("res://Scenes/move_speed_potion.tscn");
		this.itemScenes[PotionTypeEnum.ShootSpeed] = GD.Load<PackedScene>("res://Scenes/shoot_speed_potion.tscn");
	}

	public PackedScene GetItem(PotionTypeEnum key)
	{
		return this.itemScenes.TryGetValue(key, out var scene) ? scene : null;
	}

	public int GetItemCount()
	{
		return this.itemScenes.Count;
	}
}
