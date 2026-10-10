using Godot;
using System.Collections.Generic;
using Metroidvania.game.interfaces;

public partial class Level : Node2D
{
	private PackedScene bulletScene = GD.Load<PackedScene>("res://game/bullets/Bullet.tscn");
	private PackedScene playerScene = GD.Load<PackedScene>("res://game/enteties/player/Player.tscn");
	private readonly Dictionary<EnemyEnums, PackedScene> enemyScenes = new()
	{
		{EnemyEnums.Drone, GD.Load<PackedScene>("res://game/enteties/Enemies/Drone.tscn")},
		{EnemyEnums.Soldier, GD.Load<PackedScene>("res://game/enteties/Enemies/Soldier.tscn")},
	};

	private Node2D bulletContainer;
	private Node2D entitiesContainer;
	private Node2D enemySpawns;
	private Marker2D playerSpawnPoint;
	private Marker2D startPoint;
	public AudioStreamPlayer2D GunSound {get; set;}
	public AudioStreamPlayer2D ShotgunSound {get; set;}
	public AudioStreamPlayer2D ExplosionSound {get; set;}
	

	public override void _Ready()
	{
		this.startPoint = this.GetNode<Marker2D>("Gates/StartPoint");
		this.playerSpawnPoint = this.GetNode<Marker2D>("PlayerSpawnPoint");
		this.bulletContainer = this.GetNode<Node2D>("Bullets");
		this.entitiesContainer = this.GetNode<Node2D>("Entities");
		this.enemySpawns = this.GetNode<Node2D>("EnemySpawns");
		this.playerSpawnPoint.GlobalPosition = this.startPoint.GlobalPosition;
		var player = this.playerScene.Instantiate<Player>();
		player.Position = this.playerSpawnPoint.Position;
		this.entitiesContainer.AddChild(player);
		this.GunSound = this.GetNode<AudioStreamPlayer2D>("Sounds/GunSound");
		this.ShotgunSound = this.GetNode<AudioStreamPlayer2D>("Sounds/ShotgunSound");
		this.ExplosionSound = this.GetNode<AudioStreamPlayer2D>("Sounds/ExplosionSound");

		foreach (Marker2D spawnPoint in this.enemySpawns.GetChildren())
		{
			if (spawnPoint is not EnemySpawner enemySpawner || enemySpawner.IsDefeated)
			{
				continue;
			}

			var enemyScene = this.enemyScenes[enemySpawner.EnemyType].Instantiate();

			if (enemyScene is IEnemy enemy)
			{
				enemy.Setup(enemySpawner);
				this.entitiesContainer.AddChild(enemyScene);
			}
		}
	}
}
