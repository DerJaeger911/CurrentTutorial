using Godot;
using Godot.Collections;
using GodotRTSCourse.Scripts.Game.EnumAndConsts;

namespace GodotRTSCourse.Scripts.Game;

public partial class GameManager : Node2D
{
    private Dictionary<TeamEnums, int> units = new()
    {
        { TeamEnums.Player, 0 },
        { TeamEnums.Ai, 0 }
    };

    public override void _Ready()
    {
        foreach (var node in this.GetTree().GetNodesInGroup("Units"))
        {
            Unit unit = node as Unit;

            if (unit == null && node is Node parentNode)
            {
                unit = parentNode.GetNodeOrNull<Unit>(".");
                if (unit == null)
                {
                    unit = parentNode.GetNodeOrNull<Unit>("Unit");
                }
            }

            if (unit == null)
                continue;

            if (!this.units.ContainsKey(unit.Team))
            {
                this.units[unit.Team] = 0;
            }

            this.units[unit.Team] += 1;
            unit.DeathSignal += this.OnUnitDie;
        }

        GD.Print($"GameManager Ready. Units Alive: Player={this.units[TeamEnums.Player]}, AI={this.units[TeamEnums.Ai]}");
    }

    private void OnUnitDie(Unit unit)
    {
        unit.DeathSignal -= this.OnUnitDie;
        this.units[unit.Team] -= 1;
        this.CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        TeamEnums? winner = null;
        int teamsAlive = 0;

        foreach (var kvp in this.units)
        {
            TeamEnums team = kvp.Key;
            int count = kvp.Value;

            if (count > 0)
            {
                teamsAlive++;
                winner = team;
            }

            if (teamsAlive > 1)
                return;
        }

        if (winner != null)
            GD.Print("🏆 Team ", winner, " wins!");
    }
}
