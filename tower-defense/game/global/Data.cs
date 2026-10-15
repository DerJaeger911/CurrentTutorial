using Godot;
using System.Collections.Generic;

public partial class Data : Node
{
    private int money = 50;

    private Ui uiNode;

    public int CurrentWave { get; set; }

    public int Money 
    {
        get => this.money;

        set
        {
            this.money = value;
            
            if (this.uiNode != null)
            {
                this.uiNode.UpdateStats(this.money);
            }
            foreach (TowerCard towerCard in this.GetTree().GetNodesInGroup("TowerCard"))
            {
                towerCard.ToggleActive(value);
            }
        }
    }

    public static Data Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public void LateLoad(Ui uiNode)
    {
                this.uiNode = uiNode;
        GD.Print(this.uiNode);
    }

    public static readonly Dictionary<TowerTypeEnums, Dictionary<string, object>> TowerData = new()
    {
        {
            TowerTypeEnums.Basic, new Dictionary<string, object>
            {
                { "name", "Basic" },
                { "cost", 20 },
                { "upgrade_cost", 30 },
                { "reload_time", 1.0f },
                { "bullet", BulletTypeEnums.Single },
                { "thumbnail", "res://assets/graphics/ui/tower thumbnails/basic.png" },
                { "scene", "res://scenes/towers/single_tower.tscn" }
            }
        },
        {
            TowerTypeEnums.Blast, new Dictionary<string, object>
            {
                { "name", "Blaster" },
                { "cost", 30 },
                { "upgrade_cost", 50 },
                { "reload_time", 1.5f },
                { "bullet", BulletTypeEnums.Fire },
                { "thumbnail", "res://assets/graphics/ui/tower thumbnails/blaster.png" },
                { "scene", "res://scenes/towers/blaster_tower.tscn" }
            }
        },
        {
            TowerTypeEnums.Mortar, new Dictionary<string, object>
            {
                { "name", "Mortar" },
                { "cost", 30 },
                { "upgrade_cost", 60 },
                { "reload_time", 2.0f },
                { "bullet", BulletTypeEnums.MortarExplosion },
                { "thumbnail", "res://assets/graphics/ui/tower thumbnails/mortar.png" },
                { "scene", "res://scenes/towers/mortar_tower.tscn" }
            }
        }
    };

    public static readonly Dictionary<TowerTypeEnums, Dictionary<string, object>> UpgradeData = new()
    {
        {
            TowerTypeEnums.Basic, new Dictionary<string, object>
            {
                { "reload_time", 0.6f },
                { "bullet", BulletTypeEnums.Single }
            }
        },
        {
            TowerTypeEnums.Blast, new Dictionary<string, object>
            {
                { "reload_time", 1.2f },
                { "bullet", BulletTypeEnums.Fire }
            }
        },
        {
            TowerTypeEnums.Mortar, new Dictionary<string, object>
            {
                { "reload_time", 1.5f },
                { "bullet", BulletTypeEnums.IceExplosion }
            }
        }
    };

    public static readonly Dictionary<int, Dictionary<EnemyTypeEnums, int>> EnemyWaves = new()
    {
        { 0, new Dictionary<EnemyTypeEnums, int> { { EnemyTypeEnums.Default, 5 }, { EnemyTypeEnums.Strong, 2 }, { EnemyTypeEnums.Fast, 1 } } },
        { 1, new Dictionary<EnemyTypeEnums, int> { { EnemyTypeEnums.Default, 5 }, { EnemyTypeEnums.Fast, 1 } } }
    };

    public static readonly Dictionary<EnemyTypeEnums, Dictionary<string, object>> EnemyData = new()
    {
        {
            EnemyTypeEnums.Default, new Dictionary<string, object>
            {
                { "health", 3 },
                { "texture", "res://assets/graphics/Ships/ship_0004.png" },
                { "speed", 20 }
            }
        },
        {
            EnemyTypeEnums.Fast, new Dictionary<string, object>
            {
                { "health", 3 },
                { "texture", "res://assets/graphics/Ships/ship_0007.png" },
                { "speed", 50 }
            }
        },
        {
            EnemyTypeEnums.Strong, new Dictionary<string, object>
            {
                { "health", 6 },
                { "texture", "res://assets/graphics/Ships/ship_0000.png" },
                { "speed", 25 }
            }
        },
        {
            EnemyTypeEnums.Big, new Dictionary<string, object>
            {
                { "health", 20 },
                { "texture", "res://assets/graphics/Ships/ship_0015.png" },
                { "speed", 15 }
            }
        }
    };
}