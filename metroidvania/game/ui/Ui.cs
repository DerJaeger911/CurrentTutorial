using Godot;
using System;

public partial class Ui : CanvasLayer
{
	[Export]
	private TextureProgressBar healthbar;

	public void Init(int currentHealth, int maxHealth)
	{
		if (this.healthbar == null) return;
		this.healthbar.MaxValue = maxHealth;
		this.healthbar.Value = currentHealth;
	}

	public void UpdateHealth(int health)
	{
		this.healthbar.Value = health;
	}
}