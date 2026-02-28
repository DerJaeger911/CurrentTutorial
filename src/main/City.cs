using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using twentyfourtyeight.src.main;

public partial class City : Node2D
{
	private Label label;
	private Sprite2D sprite;

	public HexTileMap Map {  get; set; }
	public Vector2I CenterCoordinates { get; set; }
	public List<Hex> Territory {  get; set; } = new List<Hex>();
	public List<Hex> BorderTilePool { get; set; } = new List<Hex>();
	public string CityName { get; set; }
	public Civilisation Civ {  get; set; }

	public override void _Ready()
	{
		this.label = this.GetNode<Label>("Label");
		this.sprite = this.GetNode<Sprite2D>("Sprite2D");
		this.label.Text = this.CityName;
	}

	public void AddTerritory(List<Hex> territoryToAdd)
	{
		foreach (Hex hex in territoryToAdd) 
		{
			hex.OwnerCity = this;
		}

		this.Territory.AddRange(territoryToAdd);
	} 

	public void SetCityName(string cityName)
	{
		this.CityName = cityName;
		this.label.Text = cityName;
	}

	public void SetIconColor(Color color)
	{
		this.sprite.Modulate = color;
	}
}
