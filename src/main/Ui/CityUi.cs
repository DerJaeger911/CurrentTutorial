using Godot;
using System;
using twentyfourtyeight.src.main.SignalHubs;

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

		UISignals.OnEndTurn += this.Refresh;
	}

	public void SetCityUi(City city)
	{
		this.city = city;

		this.Refresh();
	}

	public void Refresh()
	{
		if (this.city == null)
		{
			return;
		}

		this.cityName.Text = this.city.Name;
		this.population.Text = "Population: " + this.city.Population;
		this.food.Text = "Food: " + this.city.TotalFood;
		this.production.Text = "Production: " + this.city.TotalProduction;
		GD.Print("Update");
	}

	public override void _ExitTree()
	{
		UISignals.OnEndTurn -= this.Refresh;
	}
}
