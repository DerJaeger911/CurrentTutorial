using Godot.Collections;

namespace RoguelikeCourse.Scripts.Manager.PreloadManagers.ItemPreloads
{
	internal static class ItemSets
	{
		public static Array<ItemEnum> Potions { get; } = new()
			{
				ItemEnum.HealthPotion,
				ItemEnum.DamagePotion,
				ItemEnum.MoveSpeedPotion,
				ItemEnum.ShootRatePotion
			};
	}
}
