using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twentyfourtyeight.src.main;

internal class Hex
{
	public Vector2I Coordinates { get; init; }

	public int Food { get; set; }
	public int Production { get; set; }

	public TerrainEnum TerrainType {  get; set; }

	public Hex(Vector2I coordinates)
	{
		this.Coordinates = coordinates;
	}

	public override String ToString()
	{
		return $"Hex({this.Coordinates.X}, {this.Coordinates.Y}) Terrain-Type: {this.TerrainType} Food: {this.Food} Production: {this.Production}";
	}
}
