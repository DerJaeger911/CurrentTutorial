using Godot;
using System;

public partial class UpgradeMenu : Control
{
	[Export]
	private Button upgradeButton;
	[Export]
	private TextureButton deleteButton;

	private Tower tower;

	public override void _Ready()
	{
		this.tower = this.GetParent<Tower>();
		this.upgradeButton.Pressed += this.OnUpgradeButtonPressed;
		this.deleteButton.Pressed += this.OnDeleteButtonPressed;
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
	
	public void Reveal()
	{
		this.Show();
	}
}
