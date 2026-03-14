using System;
using twentyfourtyeight.src.main;

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

	public void FoundCity()
	{
		if(this.Map.GetHex(this.Coords).OwnerCity is null && !City.InvalidTiles.ContainsKey(this.Map.GetHex(this.Coords)))
		{
			bool valid = true;

			foreach(Hex hex in this.Map.GetSurroundingHexes(this.Coords))
			{
				valid = hex.OwnerCity is null && !City.InvalidTiles.ContainsKey(hex);
			}

			if (valid)
			{
				this.Map.CreateCity(this.Civ, this.Coords, $"Fucked the Ground, came and spawned a Settlement at {this.Coords.X}");
				this.DestroyUnit();
			}
		}
	}
}
