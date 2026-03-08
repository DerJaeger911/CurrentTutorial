using Godot;
using System;
using twentyfourtyeight.src.main.SignalHubs;

public partial class UnitBuildButton : Button
{
	private CityUi cityUi;
	public Unit CurrentUnit { get; set; }

	public override void _Ready()
	{
		this.cityUi = this.GetOwner<CityUi>();
		this.Pressed += this.SendUnitData;
	}

	public void SendUnitData()
	{
		this.cityUi.BuiltAUnit(this.CurrentUnit);
		GD.Print("Button pressed");
	}
}
