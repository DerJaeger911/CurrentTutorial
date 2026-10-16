
using Godot;
using System;

public partial class TowerBasic : Tower
{

    private Sprite2D turret;

   

    public override void _Ready()
    {
        base._Ready();
        this.turret = this.GetNode<Sprite2D>("Turret");
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

}
