using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Level : Node2D
{
	private PackedScene bulletScene = GD.Load<PackedScene>("res://game/bullets/Bullet.tscn");
	private PackedScene playerScene = GD.Load<PackedScene>("res://game/enteties/player/Player.tscn");
	private Node2D bulletContainer;
	private Node2D entitiesContainer;
	private Marker2D playerSpawnPoint;

	public override void _Ready()
	{
		this.playerSpawnPoint = this.GetNode<Marker2D>("PlayerSpawnPoint");
		this.bulletContainer = this.GetNode<Node2D>("Bullets");
		this.entitiesContainer = this.GetNode<Node2D>("Entities");
		var player = this.playerScene.Instantiate<Player>();
		player.Position = this.playerSpawnPoint.Position;
		this.entitiesContainer.AddChild(player);
	}
}
