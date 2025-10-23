using Godot;
using GodotRTSCourse.Scripts.Game;

namespace GodotRTSCourse.Scripts.Game;

public partial class UnitHealthBar : ProgressBar
{
	private Unit unit;

	public override void _Ready()
	{
		this.unit = GetParent<Unit>();
		this.MaxValue = this.unit.MaxHealth;
		this.Value = this.MaxValue;
		this.Visible = false;

		this.unit.Connect(Unit.SignalName.TakeDamage, new Callable(this, nameof(UpdateValue)));
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
			this.unit.Disconnect(Unit.SignalName.TakeDamage, new Callable(this, nameof(UpdateValue)));
		}
	}
}
