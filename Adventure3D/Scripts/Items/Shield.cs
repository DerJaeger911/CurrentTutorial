using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

public partial class Shield : Node3D
{
	public static ItemTypeEnum EquipmentType = ItemTypeEnum.Shield;
	public float Defense { get; set; }

	public void Setup( float defense)
	{
		this.Defense = defense;
	}
}
