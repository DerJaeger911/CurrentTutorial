using Godot;
using System;
using System.Data.SqlTypes;

public partial class TowerCard : Button
{
	[Export]
	private Label nameLabel;
	[Export]
	private Label moneyLabel;
	[Export]
	private TextureRect towerTexture;

	private Ui ui;

	private int cost;

	private TowerTypeEnums id = TowerTypeEnums.Basic;

    public override void _Ready()
    {
        this.Pressed += this.OnPressed;
		this.cost = (int)Data.TowerData[this.id]["cost"];
		this.ToggleActive(Data.Instance.Money);
    }

	public void Setup(TowerTypeEnums id, Ui ui)
	{
		this.id = id;
		this.ui = ui;
		this.nameLabel.Text = (string)Data.TowerData[id]["name"];
		this.moneyLabel.Text = Data.TowerData[id]["cost"].ToString();
		this.towerTexture.Texture = GD.Load<Texture2D>((string)Data.TowerData[id]["thumbnail"]);

	}

	private void OnPressed()
	{
		this.ui.SelectTower(this.id);
	}

	public void ToggleActive(int money)
	{
		this.Disabled = this.cost > money;
	}

}