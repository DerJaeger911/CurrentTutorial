using Godot;
using System;

public partial class Shield : Node3D
{
	public const string EquipmentType = "Shield";
	private float defense;

	public void Setup( float defense)
	{
		this.defense = defense;
	}
}
