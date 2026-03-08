using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using twentyfourtyeight.src.main;

public partial class Unit : Node2D
{
	public static Dictionary<Type, PackedScene> UnitSceneResources {  get; private set; }
	public static Dictionary<Type, Texture2D> UiImages {  get; private set; }

	public static void LoadUnitScenes()
	{
		UnitSceneResources = new Dictionary<Type, PackedScene> 
		{
			{typeof(Settler), ResourceLoader.Load<PackedScene>("res://src/main/Units/Settler.tscn") },
			{typeof(Warrior), ResourceLoader.Load<PackedScene>("res://src/main/Units/Warrior.tscn") },
		};
	}

	public static void LoadTextures()
	{
		UiImages = new Dictionary<Type, Texture2D>
		{
			{typeof(Settler), ResourceLoader.Load<Texture2D>("res://Assets-4XHexMap/settler_image.png") },
			{typeof(Warrior), ResourceLoader.Load<Texture2D>("res://Assets-4XHexMap/warrior_image.jpg") },
		};
	}

	public Vector2I Coords { get; set; }

	public virtual string UnitName => "Default";

	public virtual int ProductionCost => 0;

	public Civilisation civ { get; set; }

	public virtual int MaxHp {  get; set; }
	
	public virtual int Hp { get; set; }
	
	public virtual int MaxMovePoints { get; set; }

	public virtual int MovePoints { get; set; }

	public void SetCiv(Civilisation civ) 
	{ 
		this.civ = civ;
		this.GetNode<Sprite2D>("Sprite2D").Modulate = civ.TerritoryColor;
		this.civ.Units.Add(this);
	}
}
