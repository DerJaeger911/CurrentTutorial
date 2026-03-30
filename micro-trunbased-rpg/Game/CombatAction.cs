using Godot;
using System;

namespace MicroTrunbasedRPG.Game;

[GlobalClass]
public partial class CombatAction : Resource
{
	[Export]
	private string displayName;
	[Export]
	private string description;

	[Export]
	private int meleeDamage;
	[Export]
	private int healAmount;

	[Export]
	private int baseWeight = 100;

	public String DisplayName { get => this.displayName; set => this.displayName = value; }
	public String Description { get => this.description; set => this.description = value; }
}
