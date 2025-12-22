using Godot;
using RoguelikeCourse.Scripts.Manager.PreloadManagers.AssetPreloads;
using RoguelikeCourse.Scripts.Manager.PreloadManagers.ItemPreloads;
using RoguelikeCourse.Scripts.Manager.PreloadManagers.LevelPreloads;
using RoguelikeCourse.Scripts.Manager.Signals;
using System;
using System.Collections.Generic;

namespace RoguelikeCourse.Scripts.Manager.Registration;

public partial class SingletonRegistry : Node
{
	private static readonly Dictionary<Type, object> services = [];

	public override void _Ready()
	{
        Register(GameSignals.Instance);
		Register(AssetPreloads.Instance);
        Register(LevelPreloads.Instance);
		Register(ItemPreloads.Instance);

        Register(DummyManager.Instance);
        Register(GameStateManager.Instance);

	}

	private static void Register<T>(T instance)
	{
		var type = typeof(T);
		if (!services.ContainsKey(type))
		{
			services[type] = instance;
		}
	}

	public static T Get<T>() where T : class
	{
		services.TryGetValue(typeof(T), out var service);
		return service as T;
	}
}
