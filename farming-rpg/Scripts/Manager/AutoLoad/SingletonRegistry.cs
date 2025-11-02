using FarmingRpg.Scripts.Manager.AutoLoad;
using Godot;
using System;
using System.Collections.Generic;

namespace ReverseBulletHell.Scripts.Manager;

public partial class SingletonRegistry : Node
{
	private static readonly Dictionary<Type, object> services = [];

	public override void _Ready()
	{
		Register(GameManager.Instance);
		Register(GameSignals.Instance);
	}

	private static void Register<T>(T instance)
	{
		if (instance == null) 
		{
			GD.Print("Oh no");
			return; 
		}
        
		var type = typeof(T);
		if (!services.ContainsKey(type))
			services[type] = instance;
	}

	public static T Get<T>() where T : class
	{
		services.TryGetValue(typeof(T), out var service);
		return service as T;
	}
}