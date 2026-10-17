
using Godot;
using System;

public partial class TowerBasic : Tower
{
    [Export]
    private Sprite2D turret;



    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (this.Enemies.Count > 0)
        {
            this.turret.LookAt(this.Enemies[0].GlobalPosition);
            this.turret.Rotation -= (float)Math.PI / 2;
            this.ShootDir = this.turret.Rotation;
        }
    }

    protected override void OnReloadTimerTimeout()
    {
        base.OnReloadTimerTimeout();
        if (this.Enemies.Count > 0)
        {
            this.CreateBullet();
        }
    }

    public override void UpgradeTower()
    {
        base.UpgradeTower();
        this.Base.Texture = GD.Load<Texture2D>("res://assets/graphics/towers/basic/basic tower upgrade bottom.png");
        this.turret.Texture = GD.Load<Texture2D>("res://assets/graphics/towers/basic/basic tower upgrade top.png");

    }
}
