using System.Collections.Generic;
using Godot;

public partial class Ui : CanvasLayer
{
	[Export]
	private Label healthLabel;
	[Export]
	private Label moneyLabel;
	[Export]
	private Button waveButton;
	[Export]
	private TextureButton menuToggleButton;
	[Export]
	private HBoxContainer towerCardsContainer;

	private MenuStateEnums currentState = MenuStateEnums.Closed;

	private PackedScene towerCardScene = GD.Load<PackedScene>("res://game/ui/TowerCard.tscn");


	private readonly Dictionary<MenuStateEnums, Dictionary<string, string>> menuButtonTextures = new()
{
	{
		MenuStateEnums.Closed, new Dictionary<string, string>
		{
			{ "normal", "res://assets/graphics/ui/menu.png" },
			{ "pressed", "res://assets/graphics/ui/menu.png" },
			{ "hover", "res://assets/graphics/ui/menu_hover.png" }
		}
	},
	{
		MenuStateEnums.Open, new Dictionary<string, string>
		{
			{ "normal", "res://assets/graphics/ui/close_normal.png" },
			{ "pressed", "res://assets/graphics/ui/close_normal.png" },
			{ "hover", "res://assets/graphics/ui/close_hover.png" }
		}
	}
};

	public override void _Ready()
	{
		Data.Instance.LateLoad(this);
		this.waveButton.Pressed += this.OnWaveButtonPressed;
		this.ChangeButtonTexture(this.currentState);
		this.menuToggleButton.Pressed += this.OnToggleButtonPressed;
		this.ToggleTowerCards();
		foreach (TowerTypeEnums towerType in Data.TowerData.Keys)
		{
			TowerCard towerCard = this.towerCardScene.Instantiate<TowerCard>();
			towerCard.Setup(towerType, this);
			this.towerCardsContainer.AddChild(towerCard);
			towerCard.Pressed += () => this.SelectTower(towerType);
		}

	}

	public void SelectTower(TowerTypeEnums id)
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.PlaceTower, (int)id);
	}

	public void UpdateStats(int money, int health)
	{
		this.moneyLabel.Text = money.ToString();
		this.healthLabel.Text = health.ToString();
	}

	private void OnWaveButtonPressed()
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.WaveStart);
	}

	private void ChangeButtonTexture(MenuStateEnums state)
	{
		this.menuToggleButton.TextureNormal = GD.Load<Texture2D>(this.menuButtonTextures[state]["normal"]);
		this.menuToggleButton.TextureHover = GD.Load<Texture2D>(this.menuButtonTextures[state]["hover"]);
		this.menuToggleButton.TexturePressed = GD.Load<Texture2D>(this.menuButtonTextures[state]["pressed"]);

	}

	private void OnToggleButtonPressed()
	{
		this.currentState = this.currentState == MenuStateEnums.Open ? MenuStateEnums.Closed : MenuStateEnums.Open;
		this.ToggleTowerCards();
	}

	public void HideCards()
	{
		this.currentState = MenuStateEnums.Closed;
		this.ToggleTowerCards();
	}

	private void ToggleTowerCards()
	{
		this.towerCardsContainer.Visible = this.currentState == MenuStateEnums.Open;
		this.ChangeButtonTexture(this.currentState);
	}
}
