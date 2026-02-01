using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

public partial class Shield : Node3D
{
	public static ItemTypeEnum EquipmentType = ItemTypeEnum.Shield;
	private float defense;

	public void Setup( float defense)
	{
		this.defense = defense;
	}
}
