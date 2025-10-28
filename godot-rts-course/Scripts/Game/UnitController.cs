using Godot;
using Godot.Collections;
using GodotRTSCourse.Scripts.Game.EnumAndConsts;
using GodotRTSCourse.Scripts.Game.Interfaces;

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
                this.TrySelectUnit();
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                this.TryCommandUnit();
            }
        }
    }

    private void TrySelectUnit()
    {
        Unit unit = this.GetSelectedUnit();


        if (unit == null || unit.Team != TeamEnums.Player)
        {
            this.UnselectUnit();
        }
        else
        {
            this.SelectUnit(unit);
        }
    }

    private void SelectUnit(Unit unit)
    {
        this.UnselectUnit();
        this.selectedUnit = unit;

        unit.DeathSignal += this.OnSelectedUnitDeath;

        if (IsInstanceValid(unit.GetNode<PlayerUnit>(GodotConsts.PlayerUnit)))
        {
            unit.GetNode<PlayerUnit>(GodotConsts.PlayerUnit).ToggleSelectionVisual(true);
        }
    }

    private void OnSelectedUnitDeath(Unit unit)
    {
        if (unit == this.selectedUnit)
        {
            this.selectedUnit = null;
        }
    }

    private void UnselectUnit()
    {
        if (IsInstanceValid(this.selectedUnit))
        {
            this.selectedUnit.DeathSignal -= this.OnSelectedUnitDeath;
            var playerUnitNode = this.selectedUnit.GetNode<PlayerUnit>(GodotConsts.PlayerUnit);
            if (IsInstanceValid(playerUnitNode))
            {
                playerUnitNode.ToggleSelectionVisual(false);
            }
        }

        this.selectedUnit = null;

    }

    private void TryCommandUnit()
    {
        if (this.selectedUnit == null)
        {
            return;
        }

        var target = this.GetSelectedUnit();

        if (target != null)
        {
            if (target.Team != TeamEnums.Player)
            {
                this.selectedUnit.SetAttackTarget(target);
            }
        }
        else
        {
            this.selectedUnit.SetMoveToTarget(this.GetGlobalMousePosition());
        }
    }

    private Unit GetSelectedUnit()
    {
        PhysicsDirectSpaceState2D space = this.GetWorld2D().DirectSpaceState;
        PhysicsPointQueryParameters2D query = new()
        {
            Position = this.GetGlobalMousePosition(),
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
