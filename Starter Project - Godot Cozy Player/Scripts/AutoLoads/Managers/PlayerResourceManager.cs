using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Enums;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Dcozysandbox.Scripts.AutoLoads.Managers;

internal class PlayerResourceManager : IPlayerResourceManager
{
	public static IPlayerResourceManager Instance { get; set; } = new PlayerResourceManager();

	private static Dictionary<ResourceEnum, int> Stocks { get; set; } = new()
	{
		{ ResourceEnum.Apple, 0 },
		{ ResourceEnum.Wood, 5 },
		{ ResourceEnum.Fish, 0 },
		{ ResourceEnum.Corn, 1 },
		{ ResourceEnum.Tomato, 1 },
		{ ResourceEnum.Pumpkin, 1 }
	};

	private PlayerResourceManager() { }

	public void AddResource(ResourceEnum resource, int amount)
	{
		Stocks[resource] += amount;
		GD.Print($"{amount} {resource} hinzugefügt. Neuer Stand: {Stocks[resource]}");
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ResourceCountChanged);
	}

    public bool CheckResource(ResourceEnum resource, int amount)
    {
        if (Stocks[resource] - amount >= 0)
        {
			return true;
        }
        return false;
    }

	public int GetResourceCount(ResourceEnum resource)
	{
		return Stocks[resource];
	}

	public void SubtractResource(ResourceEnum resource, int amount)
	{
		Stocks[resource] -= amount;
		GD.Print($"{amount} {resource} hinzugefügt. Neuer Stand: {Stocks[resource]}");
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ResourceCountChanged);
	}

	public void PrintAll()
	{
		string report = string.Join("\n", Stocks.Select(kvp => $"- {kvp.Key}: {kvp.Value}"));

		GD.Print("--- Aktueller Lagerbestand ---");
		GD.Print(report);
		GD.Print("------------------------------");
	}
}
