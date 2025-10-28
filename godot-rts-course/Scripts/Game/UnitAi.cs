using Godot;
using Godot.Collections;
using GodotRTSCourse.Scripts.Game;
using GodotRTSCourse.Scripts.Game.EnumAndConsts;
using GodotRTSCourse.Scripts.Game.Interfaces;
using System;

namespace GodotRTSCourse.Scripts.Game;

public partial class UnitAi : Node, IUnitAi
{
    [Export]
    private float detectionRange = 100;
    [Export]
    private float detectionRate = 0.2f;
    private float lastDetectionTime;
    private Array<Unit> enemyList = [];

    private Unit unit;

    public override void _Ready()
    {
        this.unit = this.GetParent<Unit>();
    }

    public override void _Process(double delta)
    {
        float time = (float)Time.GetTicksMsec() / 1000;

        if (time - this.lastDetectionTime > this.detectionRate)
        {
            this.lastDetectionTime = time;
            this.UpdateEnemyList();
            this.Detect();
        }
    }

    private void UpdateEnemyList()
    {
        this.enemyList.Clear();

        Array<Node> rawList = this.GetTree().GetNodesInGroup(GodotConsts.PlayerUnits);

        foreach (var node in rawList)
        {
            if (node is Unit player)
                this.enemyList.Add(player);
        }
    }

    private void Detect()
    {
        Unit closestEnemy = null;
        float closestDistance = 99999;

        foreach (Unit enemy in this.enemyList)
        {
            var distance = this.unit.GlobalPosition.DistanceTo(enemy.GlobalPosition);

            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }
        if (closestEnemy != null)
        {
            this.unit.SetAttackTarget(closestEnemy);
        }
    }

}
