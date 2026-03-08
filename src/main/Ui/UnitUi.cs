using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class UnitUi : Panel
{
	private TextureRect unitImage;

	private Label unitLabel, healthLabel, movesLabel;

	private Unit unit;

	public override void _Ready()
	{
		this.unitImage = this.GetNode<TextureRect>("VBoxContainer/TextureRect");
		this.unitLabel = this.GetNode<Label>("VBoxContainer/UnitTypeLabel");
		this.healthLabel = this.GetNode<Label>("VBoxContainer/HealthLabel");
		this.movesLabel = this.GetNode<Label>("VBoxContainer/MovesLabel");
	}

	public void SetUnit(Unit unit)
	{
		this.unit = unit;

		this.Refresh();
	}

	public void Refresh()
	{
		this.unitImage.Texture = Unit.UiImages[this.unit.GetType()];
		this.unitLabel.Text = this.unit.UnitName;
		this.movesLabel.Text = $"{this.unit.MovePoints}/{this.unit.MaxMovePoints}";
		this.healthLabel.Text = $"{this.unit.Hp}/{this.unit.MaxHp}";
	}
}
