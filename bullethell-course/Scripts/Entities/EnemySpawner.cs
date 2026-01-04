using bullethellcourse.Scripts.Entities;
using bullethellcourse.Scripts.Pools;
using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node
{
	[Export]
	private float startEnemiesPerSecond = 0.5f;
	[Export]
	private float enemiesPerSecondIncrease = 0.1f;


	private List<EnemyPoolBase> enemyPools = new();
	private List<int> enemySpawnWeights = new() { 10, 1 };
	private List<Node2D> spawnPoints = new();
	private float enemiesPerSecond;

	private float spawnRate;

	private Timer spawnTimer;

	public override void _Ready()
	{
		foreach (Node child in this.GetChildren())
		{
			if (child is Node2D node2D)
			{
				this.spawnPoints.Add(node2D);
			}

			if (child is EnemyPoolBase enemyPool)
			{
				this.enemyPools.Add(enemyPool);
			}
		}
		GD.Print(this.enemyPools.Count);
		this.enemiesPerSecond = this.startEnemiesPerSecond;
		this.spawnTimer = this.GetNode<Timer>("SpawnTimer");
		this.spawnTimer.Timeout += this.OnSpawnTimerTimeout;
		this.OnSpawnTimerTimeout();
	}

	public override void _Process(Double delta)
	{
		this.enemiesPerSecond += this.enemiesPerSecondIncrease * (float)delta;
		this.spawnRate = 1 / this.enemiesPerSecond;

	}

	private int GetRandomEnemyIndex()
	{
		int totalWeight = 0;

		foreach (int weight in this.enemySpawnWeights)
		{
			totalWeight += weight;
		}

		var rand = GD.Randf() * totalWeight;

		for (int i = 0; i < this.enemySpawnWeights.Count; i++)
		{
			rand -= this.enemySpawnWeights[i];

			if (rand < 0)
			{
				return i;
			}
		}

		return -1;
	}

	private void OnSpawnTimerTimeout()
	{
		Node parent = this.GetTree().CurrentScene;
		Enemy enemy = (Enemy)this.enemyPools[this.GetRandomEnemyIndex()].Spawn(parent);
		//Enemy enemy = (Enemy)this.enemyPools[1].Spawn(parent);
		Vector2 spawnpoint = this.spawnPoints[GD.RandRange(0, this.spawnPoints.Count - 1)].GlobalPosition;
		enemy.CallDeferred(nameof(Enemy.ResetFailSafe));
		enemy.GlobalPosition = spawnpoint;


		this.spawnTimer.Start(this.spawnRate);
	}
}