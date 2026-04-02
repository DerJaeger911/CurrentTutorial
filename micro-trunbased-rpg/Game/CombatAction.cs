using Godot;
using System;

namespace MicroTrunbasedRPG.Game;

[GlobalClass]
public partial class CombatAction : Resource
{
	[Export]
	public string DisplayName { get; set; }
	[Export]
	public string Description { get; set; }

	[Export]
	public int MeleeDamage { get; set; }
	[Export]
	public int HealAmount { get; set; }

	[Export]
	public int BaseWeight { get; set; } = 100;
}
