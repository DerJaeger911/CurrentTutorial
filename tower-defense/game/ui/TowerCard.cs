using Godot;
using System;
using System.Data.SqlTypes;

public partial class TowerCard : Button
{
	private Ui ui;

	private int cost;

	private TowerTypeEnums id = TowerTypeEnums.Basic;

    public override void _Ready()
    {
		this.ui = this.GetParent().GetParent().GetParent<Ui>();
        this.Pressed += this.OnPressed;
		this.cost = (int)Data.TowerData[TowerTypeEnums.Basic]["cost"];
		this.ToggleActive(Data.Instance.Money);
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