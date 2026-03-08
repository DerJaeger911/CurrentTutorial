using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using twentyfourtyeight.src.main;

public abstract partial class Unit : Node2D
{
	public static Dictionary<Type, PackedScene> UnitSceneResources;

	public static void LoadUnitScenes()
	{
		UnitSceneResources = new Dictionary<Type, PackedScene> 
		{
			{typeof(Settler), ResourceLoader.Load<PackedScene>("res://src/main/Units/Settler.tscn") },
			{typeof(Warrior), ResourceLoader.Load<PackedScene>("res://src/main/Units/Warrior.tscn") },
		};
	}

	public Vector2I Coords { get; set; }

	public abstract string UnitName { get; }

	public abstract int ProductionCost { get; }

	public Civilisation civ { get; set; }

	public void SetCiv(Civilisation civ) 
	{ 
		this.civ = civ;
		this.GetNode<Sprite2D>("Sprite2D").Modulate = civ.TerritoryColor;
		this.civ.Units.Add(this);
	}
}
