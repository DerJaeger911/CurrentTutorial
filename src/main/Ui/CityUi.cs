using Godot;
using System;

public partial class CityUi : Panel
{
	private Label cityName, population, food, production;
	private TextureRect cityImage;

	private City city;

	public override void _Ready()
	{
		VBoxContainer uiHolder = this.GetNode<VBoxContainer>("VBoxContainer");
		VBoxContainer labelHolder = uiHolder.GetNode<VBoxContainer>("MarginContainer/VBoxContainer");

		this.cityImage = uiHolder.GetNode<TextureRect>("TextureRect");
		this.cityName = labelHolder.GetNode<Label>("CityName");
		this.population = labelHolder.GetNode<Label>("Population");
		this.food = labelHolder.GetNode<Label>("Food");
		this.production = labelHolder.GetNode<Label>("Production");
	}

	public void SetCityUi(City city)
	{
		this.city = city;

		this.cityName.Text = city.Name;
		this.population.Text = "Population: " + city.Population;
		this.food.Text = "Food: " + city.TotalFood;
		this.production.Text = "Production: " + city.TotalProduction;
	}
}
