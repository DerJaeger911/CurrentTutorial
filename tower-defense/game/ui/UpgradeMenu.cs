using Godot;
using System;

public partial class UpgradeMenu : Control
{
	[Export]
	private Button upgradeButton;
	[Export]
	private TextureButton deleteButton;

	private int upgradeCost;

	private Tower tower;

	public override void _Ready()
	{
		this.tower = this.GetParent<Tower>();
		this.upgradeButton.Pressed += this.OnUpgradeButtonPressed;
		this.deleteButton.Pressed += this.OnDeleteButtonPressed;
		this.upgradeCost = this.tower.UpgradeCost;
		this.upgradeButton.Text = $"Upgrade {this.upgradeCost}";
		this.ToggleActive();
	}

	//ButtonPresses
	private void OnUpgradeButtonPressed()
	{
		this.tower.UpgradeTower();
		this.Hide();
	}

	private void OnDeleteButtonPressed()
	{
		this.tower.DeleteTower();
		this.Hide();
	}

	public void Reveal(bool upgraded)
	{
			this.Show();
			if (upgraded)
		{
			this.upgradeButton.Hide();
		}
	}

	public void ToggleActive()
	{
		this.upgradeButton.Disabled = this.upgradeCost > Data.Instance.Money;
	}
}
