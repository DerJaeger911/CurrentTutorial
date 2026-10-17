using Godot;
using System;

public partial class TowerMortar : Tower
{
	private bool justInit = true;
	[Export]
	private Sprite2D crosshairSprite;
	private bool crossHairSet;

	public override void _Ready()
	{
		base._Ready();
		this.crosshairSprite.Hide();
	}

	protected override void OnSelectedPlacedTower()
	{
		base.OnSelectedPlacedTower();
		this.ShowCrosshair();
	}

	private void ShowCrosshair()
	{
		if (!this.justInit)
		{
			this.crosshairSprite.Show();
		}
		else
		{
			this.justInit = false;
		}
	}

	public void FinishPlacing()
	{
		this.crosshairSprite.Hide();
	}

	public void CrosshairPosUpdate(Vector2I crosshairPos)
	{
		this.crosshairSprite.GlobalPosition = crosshairPos;
		this.crossHairSet = true;
	}

	protected override void OnReloadTimerTimeout()
	{
		this.CreateBullet(this.crosshairSprite.GlobalPosition);
	}

	public override void UpgradeTower()
	{
		base.UpgradeTower();
        this.Base.Texture = GD.Load<Texture2D>("res://assets/graphics/towers/mortar/mortar tower upgrade down.png");

	}
}
