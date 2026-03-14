using Godot;
using System;
using System.Collections.Generic;

namespace twentyfourtyeight.src.main;

public partial class Civilisation
{
	public int Id { get; set; }
	public List<City> Cities { get; set; } = new List<City>();
	public List<Unit> Units { get; set; } = new();
	public Color TerritoryColor { get; set; }
	public int TerritoryColorAltTileId { get; set; } 
	public string TerritoryName { get; set; }
	public bool IsPlayerCivilisation { get; set; }

	public int MaxUnits { get; set; } = 3;

	public void ProcessTurn()
	{
		foreach(City city in this.Cities)
		{
			city.ProcessTurn();
		}

		if (!this.IsPlayerCivilisation)
		{
			Random rnd = new();
			foreach(City city in this.Cities)
			{
				int rndInt = rnd.Next(30);

				if(rndInt > 27)
				{
					city.AddUnitToBuildQueue(new Warrior());
				}

				if(rndInt > 28)
				{
					city.AddUnitToBuildQueue(new Settler());
				}
			}

			List<Settler> citiesToFound = new();
				foreach(Unit unit in this.Units)
			{
				unit.RandomMove();

				if(unit is Settler && rnd.Next(10) > 8)
				{
					Settler settler = (Settler)unit;
					citiesToFound.Add(settler);
				}
			}

				for(int i =0; i < citiesToFound.Count; i++)
			{
				citiesToFound[i].FoundCity();
			}
		}

		this.MaxUnits = this.Cities.Count * 3;
	}
}
