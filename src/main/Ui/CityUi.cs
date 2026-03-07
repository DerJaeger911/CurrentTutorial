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
		VBoxContainer uiContainer = this.GetNode<VBoxContainer>("VBoxContainer");
		VBoxContainer outerContainer = uiContainer.GetNode<VBoxContainer>("MarginContainer/OuterContainer");
		VBoxContainer cityContainer = outerContainer.GetNode<VBoxContainer>("CityContainer");
		VBoxContainer buildContainer = outerContainer.GetNode<VBoxContainer>("BuildContainer");
		VBoxContainer queueContainer = outerContainer.GetNode<VBoxContainer>("QueueContainer");

		this.cityImage = uiContainer.GetNode<TextureRect>("TextureRect");
		this.cityName = cityContainer.GetNode<Label>("CityName");
		this.population = cityContainer.GetNode<Label>("Population");
		this.food = cityContainer.GetNode<Label>("Food");
		this.production = cityContainer.GetNode<Label>("Production");

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
