using Godot;
using System;

namespace RoguelikeCourse.Scripts.Items
{
	internal static class PrintStats
	{
		public static void AllStats(Player player, StatEnum changedStat)
		{
			foreach (StatEnum value in Enum.GetValues<StatEnum>())
			{
				switch (value)
				{
					case StatEnum.Health:
						GD.Print($"{value}: {player.CurrentHP}");
						break;
					case StatEnum.Damage:
						GD.Print($"{value}: {player.AttackDamage}");
						break;
					case StatEnum.ShootRate:
						GD.Print($"{value}: {player.ShootRate}");
						break;
					case StatEnum.MoveSpeed:
						GD.Print($"{value}: {player.MoveSpeed}");
						break;
				}
			}

		}
	}
}
