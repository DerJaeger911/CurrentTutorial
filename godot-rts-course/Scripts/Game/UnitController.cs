using Godot;
using Godot.Collections;
using GodotRTSCourse.Scripts.Game;

namespace GodotRTSCourse.Scripts.Game;
public partial class UnitController : Node2D
{
	private Unit selectedUnit;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				TrySelectUnit();
			}
			else if (mouseEvent.ButtonIndex == MouseButton.Right)
			{
				TryCommandUnit();
			}
		}
	}

	private void TrySelectUnit()
	{
		Unit unit = GetSelectedUnit();


		if (unit == null || unit.Team != TeamEnums.Player)
		{
			UnselectUnit();
		}
		else
		{
			SelectUnit(unit);
		}
	}

	private void SelectUnit(Unit unit)
	{
		
		UnselectUnit();
		this.selectedUnit = unit;
		unit.GetNode<PlayerUnit>("PlayerUnit").ToggleSelectionVisual(true);
	}

	private void UnselectUnit()
	{
		this.selectedUnit?.GetNode<PlayerUnit>("PlayerUnit").ToggleSelectionVisual(false);
		this.selectedUnit = null;
	}

	private void TryCommandUnit()
	{
		if (this.selectedUnit == null)
		{
			return;
		}

		var target = GetSelectedUnit();

		if (target != null)
		{
			if (target.Team != TeamEnums.Player) 
			{
				this.selectedUnit.SetAttackTarget(target);
			}
		}
		else
		{
			this.selectedUnit.SetMoveToTarget(GetGlobalMousePosition());
		}
	}

	private Unit GetSelectedUnit()
	{
		PhysicsDirectSpaceState2D space = GetWorld2D().DirectSpaceState;
		PhysicsPointQueryParameters2D query = new()
		{
			Position = GetGlobalMousePosition(),
			CollideWithAreas = true
		};

		Array<Dictionary> intersection = space.IntersectPoint(query, 1);

		if (intersection.Count == 0)
		{
			return null;
		}

		GodotObject collider = intersection[0][GodotConsts.collider].AsGodotObject();

		if (collider is not Unit unit)
		{
			return null;
		}

		return unit;
	}
}
