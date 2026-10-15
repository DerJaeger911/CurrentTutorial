using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Ui : CanvasLayer
{
	[Export]
	private Label healthLabel;
	[Export]
	private Label moneyLabel;
	[Export]
	private Button waveButton;

	private int wave;

	public override void _Ready()
	{
		Data.Instance.LateLoad(this);
		this.waveButton.Pressed += this.OnWaveButtonPressed;
	}

	public void SelectTower(TowerTypeEnums id)
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.PlaceTower, (int)id);
	}

	public void UpdateStats(int money)
	{
		this.moneyLabel.Text = money.ToString();
		GD.Print(money);
	}

	private void OnWaveButtonPressed()
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.WaveStart);
	}
}
