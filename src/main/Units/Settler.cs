using Godot;
using System;

public partial class Settler : Unit
{
	public override string UnitName => "Settler";
	public override Int32 ProductionCost => 100;

	public override Int32 MaxHp { get; set; } = 1;
	public override Int32 MaxMovePoints { get; set; } = 2;

	public override void _Ready()
	{
		base._Ready();
		this.Hp = this.MaxHp;
		this.MovePoints = this.MaxMovePoints;
	}
}
