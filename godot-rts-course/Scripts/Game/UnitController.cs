using Godot;
using Godot.Collections;
using GodotRTSCourse.Scripts.Game;

public partial class UnitController : Node2D
{
    private Unit selectedUnit;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if(mouseEvent.ButtonIndex == MouseButton.Left)
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

    }

    private void UnselectUnit()
    {

    }

    private void TryCommandUnit()
    {

    }

    private Unit GetSelectedUnit()
    {
        PhysicsDirectSpaceState2D space = GetWorld2D().DirectSpaceState;
        PhysicsPointQueryParameters2D query = new();
        query.Position = GetGlobalMousePosition();
        Array<Dictionary> intersection = space.IntersectPoint(query, 1);

        GodotObject collider = intersection[0][GodotConsts.collider].AsGodotObject();
        

        if (intersection.Count == 0 || collider is not Unit unit)
        {
            return null;
        }

        return unit;
    }
}
