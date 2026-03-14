using Godot;
using twentyfourtyeight.src.main.SignalHubs;

public partial class CityUi : Panel
{
	private Label cityName, population, food, production;
	private TextureRect cityImage;
	private VBoxContainer queueBox;
	private UnitBuildButton settlerButton, warriorButton;

	private City city;

	public override void _Ready()
	{
		VBoxContainer uiContainer = this.GetNode<VBoxContainer>("VBoxContainer");
		VBoxContainer outerContainer = uiContainer.GetNode<VBoxContainer>("MarginContainer/OuterContainer");
		VBoxContainer cityContainer = outerContainer.GetNode<VBoxContainer>("CityContainer");
		VBoxContainer buildContainer = outerContainer.GetNode<VBoxContainer>("BuildContainer");
		VBoxContainer queueContainer = outerContainer.GetNode<VBoxContainer>("QueueContainer");
		VBoxContainer buildButtonBox = buildContainer.GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");

		this.queueBox = queueContainer.GetNode<VBoxContainer>("ScrollContainer2/VBoxContainer");
		this.settlerButton = buildButtonBox.GetNode<UnitBuildButton>("SettlerButton");
		this.warriorButton = buildButtonBox.GetNode<UnitBuildButton>("WarriorButton");

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

		this.settlerButton.CurrentUnit = new Settler();
		this.warriorButton.CurrentUnit = new Warrior();
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

		this.PopulateUnitQueueUi(this.city);
	}

	public void BuiltAUnit(Unit unit)
	{
		this.city.AddUnitToBuildQueue(unit);
		this.Refresh();
	}

	public void PopulateUnitQueueUi(City city)
	{
		foreach(Node node in this.queueBox.GetChildren())
		{
			this.queueBox.RemoveChild(node);
			node.QueueFree();
		}

		for (int i = 0; i < city.UnitBuildQueue.Count; i++)
		{
			Unit unit = city.UnitBuildQueue[i];
			if (i == 0)
			{
				this.queueBox.AddChild(new Label()
				{
					Text = $"{unit.UnitName} {this.city.UnitBuildTracker}/{unit.ProductionCost}"
				});
			}
			else
			{
				this.queueBox.AddChild(new Label()
				{
					Text = $"{unit.UnitName} 0/{unit.ProductionCost}"
				});
			}
		}
	}

	public override void _ExitTree()
	{
		UISignals.OnEndTurn -= this.Refresh;
	}
}
