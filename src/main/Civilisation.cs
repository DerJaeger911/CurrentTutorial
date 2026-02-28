using Godot;
using System.Collections.Generic;

namespace twentyfourtyeight.src.main;

public partial class Civilisation
{
	public int Id { get; set; }
	public List<City> Cities { get; set; } = new List<City>();
	public Color TerritoryColor { get; set; }
	public int TerritoryColorAltTileId { get; set; } 
	public string TerritoryName { get; set; }
	public bool PlayerCivilisation { get; set; }

	public Civilisation()
	{

	}
}
