using Godot;
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

	public void ProcessTurn()
	{
		foreach(City city in this.Cities)
		{
			city.ProcessTurn();
		}
	}
}
