using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethellcourse.Scripts.Managers;

internal class EnemyLoadManager
{
	private static EnemyLoadManager instance = new();
	public static EnemyLoadManager Instance => instance;

	private Dictionary<EnemyTypeEnum, PackedScene> enemyScenes = new();

	private EnemyLoadManager()
	{
        this.enemyScenes[EnemyTypeEnum.FireGhost] = GD.Load<PackedScene>("res://Scenes/enemy.tscn");
		this.enemyScenes[EnemyTypeEnum.Boss] = GD.Load<PackedScene>("res://Scenes/boss_enemy.tscn");
	}

	public PackedScene GetEnemy(EnemyTypeEnum key)
	{
		return this.enemyScenes.TryGetValue(key, out var scene) ? scene : null;
	}
}
