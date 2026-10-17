using Godot;
using System;
using System.Threading.Tasks;

public partial class TowerMortar : Tower
{
	[Export]
	private Sprite2D crosshairSprite;
	[Export]
	private AnimatedSprite2D shootAnimation;

	private bool justInit = true;

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

	protected override async void OnReloadTimerTimeout()
	{
		base.OnReloadTimerTimeout();
		this.shootAnimation.Show();
		this.shootAnimation.Play();
		await this.ToSignal(this.shootAnimation, AnimatedSprite2D.SignalName.AnimationFinished);
		this.CreateBullet(this.crosshairSprite.GlobalPosition);
	}

	public override void UpgradeTower()
	{
		base.UpgradeTower();
		this.Base.Texture = GD.Load<Texture2D>("res://assets/graphics/towers/mortar/mortar tower upgrade down.png");

	}
}
