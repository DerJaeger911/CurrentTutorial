using Godot;
using GodotRTSCourse.Scripts.Game;

namespace GodotRTSCourse.Scripts.Game;

public partial class UnitHealthBar : ProgressBar
{
    private Unit unit;

    public override void _Ready()
    {
        this.unit = this.GetParent<Unit>();
        this.MaxValue = this.unit.MaxHealth;
        this.Value = this.MaxValue;
        this.Visible = false;

        this.unit.TakeDamage += this.UpdateValue;
    }

    private void UpdateValue(int health)
    {
        this.Value = health;
        this.Visible = this.Value < this.MaxValue;
    }

    public override void _ExitTree()
    {
        if (IsInstanceValid(this.unit))
        {
            this.unit.TakeDamage -= this.UpdateValue;
        }
    }
}
