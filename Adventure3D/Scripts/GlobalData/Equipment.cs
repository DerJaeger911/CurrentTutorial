using Adventuregame.Scripts.GlobalData.Enums;
using Adventuregame.Scripts.GlobalData.ObjectDataClasses;
using Godot;
using System.Collections.Generic;

namespace Adventuregame.Scripts.GlobalData;

public partial class Equipment : Node
{
	public Dictionary<WeaponEnum, WeaponData> Weapons { get; private set; }
	public Dictionary<ShieldEnum, ShieldData> Shields { get; private set; }
	public Dictionary<StyleEnum, StyleData> Styles { get; private set; }

	public static Equipment Instance { get; private set; }

	override public void _Ready()
	{
		Instance = this;
		this.Weapons = new() {
			{ WeaponEnum.Dagger, this.CreateWeapon("dagger", 1, 1.2f, "1H_Melee_Attack_Stab") },
			{ WeaponEnum.Sword,  this.CreateWeapon("sword", 2, 1.5f, "1H_Melee_Attack_Slice_Horizontal") },
			{ WeaponEnum.Axe,    this.CreateWeapon("axe", 3, 1.3f, "2H_Melee_Attack_Spin") },
			{ WeaponEnum.Staff,  this.CreateWeapon("staff", 1, 2.1f, "2H_Melee_Attack_Slice") },
		};

		this.Shields = new() {
			{ ShieldEnum.Square, this.CreateShield("square", 0.8f) },
			{ ShieldEnum.Round,  this.CreateShield("round", 0.9f) },
			{ ShieldEnum.Spike,  this.CreateShield("spike", 0.6f) },
		};

		this.Styles = new() {
			{ StyleEnum.Sunglasses, this.CreateStyle("sunglasses") },
			{ StyleEnum.Starglasses, this.CreateStyle("starglasses") },
			{ StyleEnum.Duckhat, this.CreateStyle("duckhat") },
			{ StyleEnum.Tophat, this.CreateStyle("tophat") },
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
