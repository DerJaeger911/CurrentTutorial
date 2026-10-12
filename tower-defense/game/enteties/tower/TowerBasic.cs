

using System;

public partial class TowerBasic : Tower
{

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
        }
    }
}
