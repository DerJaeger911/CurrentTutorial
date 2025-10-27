using Godot;
using Godot.Collections;
using GodotRTSCourse.Scripts.Game;
using GodotRTSCourse.Scripts.Game.Interfaces;
using System;

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
        unit = GetParent<Unit>();
    }

    public override void _Process(double delta)
    {
        float time = (float)Time.GetTicksMsec() / 1000;

        if (time - lastDetectionTime > detectionRate)
        {
            lastDetectionTime = time;
            UpdateEnemyList();
            Detect();
        }
    }

    private void UpdateEnemyList()
    {
        enemyList.Clear();

        Array<Node> rawList = GetTree().GetNodesInGroup("PlayerUnits");

        foreach (var node in rawList)
        {
            if (node is Unit player)
                enemyList.Add(player);
        }
    }

    private void Detect()
    {
        Unit closestEnemy = null;
        float closestDistance = 99999;

        foreach(Unit enemy in enemyList)
        {
            var distance = unit.GlobalPosition.DistanceTo(enemy.GlobalPosition);

            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }
        if (closestEnemy != null) 
        {
            unit.SetAttackTarget(closestEnemy);
        }
    }

}
