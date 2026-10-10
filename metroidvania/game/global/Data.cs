using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;

public partial class Data : Node
{

	public static Data Instance { get; private set; }

	public LevelEnums CurrentLevel {get; set;} = LevelEnums.SubwayLevel;

	public int PlayerHealth {get; set;} = -1;
	public int PlayerMaxHealth {get; set;} = -1;

	public Dictionary<string, bool> EnemyData {get; set;} = new();

	public readonly Dictionary<LevelEnums, string> LevelPaths = new()
	{
		{LevelEnums.SubwayLevel, "res://game/level/SubwayLevel.tscn"},
		{LevelEnums.SkyLevel, "res://game/level/SkyLevel.tscn"},
		{LevelEnums.SewerLevel, "res://game/level/SewerLevel.tscn"},
	};

	override public void _Ready()
	{
		if (Instance != null)
		{
			GD.PrintErr("Multiple instances of Data detected. This is not allowed.");
			this.QueueFree();
			return;
		}
		Instance = this;

	}
}
