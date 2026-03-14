using Godot;
using twentyfourtyeight.src.main.SignalHubs;

public partial class UnitUi : Panel
{
	private TextureRect unitImage;

	private Label unitLabel, healthLabel, movesLabel;

	private Unit unit;

	private VBoxContainer actionsContainer;


    public override void _Ready()
	{
		this.unitImage = this.GetNode<TextureRect>("VBoxContainer/TextureRect");
		this.unitLabel = this.GetNode<Label>("VBoxContainer/UnitTypeLabel");
		this.healthLabel = this.GetNode<Label>("VBoxContainer/HealthLabel");
		this.movesLabel = this.GetNode<Label>("VBoxContainer/MovesLabel");
		this.actionsContainer = this.GetNode<VBoxContainer>("VBoxContainer/MarginContainer/VBoxContainer2");
		UISignals.OnEndTurn += this.Refresh;
	}

	public void SetUnit(Unit unit)
	{
		this.unit = unit;

		if(this.unit.GetType() == typeof(Settler))
		{
			Button foundCityButton = new();
			foundCityButton.Text = "Fuck Ground";
			this.actionsContainer.AddChild(foundCityButton);

			Settler settler = (Settler)this.unit;
			foundCityButton.Pressed += settler.FoundCity;
		}

		this.Refresh();
	}

	public void Refresh()
	{
		this.unitImage.Texture = Unit.UiImages[this.unit.GetType()];
		this.unitLabel.Text = this.unit.UnitName;
		this.movesLabel.Text = $"{this.unit.MovePoints}/{this.unit.MaxMovePoints}";
		this.healthLabel.Text = $"{this.unit.Hp}/{this.unit.MaxHp}";
	}

    public override void _ExitTree()
    {
        UISignals.OnEndTurn -= this.Refresh;
    }
}
