using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twentyfourtyeight.src.main;

internal class Hex
{
	public readonly Vector2I Coordinates;

	public TerrainEnum TerrainType {  get; set; }

	public Hex(Vector2I coordinates)
	{
		this.Coordinates = coordinates;
	}

	public override String ToString()
	{
		return $"Hex({this.Coordinates.X}, {this.Coordinates.Y}) - {this.TerrainType}";
	}
}
