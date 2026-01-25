using Adventuregame.Scripts.Global.ObjectClasses;
using Adventuregame.Scripts.Global.ObjectDataClasses;
using Godot;
using System;
using System.Collections.Generic;

namespace Adventuregame.Scripts.Global;

public partial class Global : Node
{
	public Dictionary<string, WeaponData> Weapons;
	public Dictionary<string, ShieldData> Shields;
	public Dictionary<string, StyleData> Style;

	override public void _Ready()
	{

		this.Weapons = new() {
			{ "dagger", this.CreateWeapon("dagger", 1, 1.2f, "1H_Melee_Attack_Stab") },
			{ "sword",  this.CreateWeapon("sword", 2, 1.5f, "1H_Melee_Attack_Slice_Horizontal") },
			{ "axe",    this.CreateWeapon("axe", 3, 1.3f, "2H_Melee_Attack_Spin") },
			{ "staff",  this.CreateWeapon("staff", 1, 2.1f, "2H_Melee_Attack_Slice") },
		};

		this.Shields = new() {
			{ "square", this.CreateShield("square", 0.8f) },
			{ "round",  this.CreateShield("round", 0.9f) },
			{ "spike",  this.CreateShield("spike", 0.6f) },
		};

		this.Style = new() {
			{ "sunglasses", this.CreateStyle("sunglasses") },
			{ "starglasses", this.CreateStyle("starglasses") },
			{ "duckhat", this.CreateStyle("duckhat") },
			{ "tophat", this.CreateStyle("tophat") },
		};
	}

	private WeaponData CreateWeapon(string name, int damage, float range, string anim)
	{
		return new WeaponData
		{
			Damage = damage,
			Range = range,
			Animation = anim,
			Thumbnail = GD.Load<Texture2D>($"res://graphics/ui/thumbnails/{name}.png"),
			Scene = GD.Load<PackedScene>($"res://scenes/weapons/{name}.tscn"),
			Audio = GD.Load<AudioStream>($"res://audio/{name}_sound.wav"),
		};
	}

	private ShieldData CreateShield(string name, float defense)
	{
		return new ShieldData
		{
			Defense = defense,
			Thumbnail = GD.Load<Texture2D>($"res://graphics/ui/thumbnails/{name}.png"),
			Scene = GD.Load<PackedScene>($"res://scenes/shields/{name}_shield.tscn"),
		};
	}
	private StyleData CreateStyle(string name)
	{
		return new StyleData
		{
			Thumbnail = GD.Load<Texture2D>($"res://graphics/ui/thumbnails/{name}.png"),
			Scene = GD.Load<PackedScene>($"res://scenes/style/{name}.tscn"),
		};
	}
}
