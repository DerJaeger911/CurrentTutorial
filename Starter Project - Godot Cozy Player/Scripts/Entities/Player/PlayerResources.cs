using Dcozysandbox.Scripts.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.Entities.Player;

internal class PlayerResources
{
	public static Dictionary<ResourceEnum, int> Stocks { get; set; } = new()
	{
		{ ResourceEnum.Apple, 0 },
		{ ResourceEnum.Wood, 0 },
		{ ResourceEnum.Fish, 0 },
		{ ResourceEnum.Corn, 0 },
		{ ResourceEnum.Tomato, 0 },
		{ ResourceEnum.Pumpkin, 0 }
	};

	public static void AddResource(ResourceEnum resource, int amount)
	{
		Stocks[resource] += amount;
		GD.Print($"{amount} {resource} hinzugefügt. Neuer Stand: {Stocks[resource]}");
	}
}
