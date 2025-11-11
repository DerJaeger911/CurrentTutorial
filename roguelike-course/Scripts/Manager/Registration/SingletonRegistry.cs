using Godot;
using RoguelikeCourse.Scripts.Manager;
using System;
using System.Collections.Generic;

public partial class SingletonRegistry : Node
{
	private static readonly Dictionary<Type, object> services = [];

	public override void _Ready()
	{
		Register(DummyManager.Instance);
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
