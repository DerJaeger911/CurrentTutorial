using Godot;
using MicroTrunbasedRPG.Game;

public partial class HealthBar : ProgressBar
{
	private Label hpLabel;

	override public void _Ready()
	{
		this.hpLabel = this.GetNode<Label>("Label");
	}

	public void UpdateMaxHealth(int maxHealth)
	{
		this.MaxValue = maxHealth;
		this.UpdateHealthLabel();
	}

	public void UpdateHealthBar(int health)
	{
		this.Value = health;
		this.UpdateHealthLabel();
	}

	private void UpdateHealthLabel()
	{
		this.hpLabel.Text = $"{this.Value}/{(int)this.MaxValue}";
	}
}
