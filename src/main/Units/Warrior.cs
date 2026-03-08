using Godot;
using System;

public partial class Warrior : Unit
{
	public override string UnitName => "Warrior";
	public override Int32 ProductionCost => 50;

	public override Int32 MaxHp { get; set; } = 3;
	public override Int32 MaxMovePoints { get; set; } = 1;

	public override void _Ready()
	{
		this.Hp = this.MaxHp;
		this.MovePoints = this.MaxMovePoints;
	}
}
