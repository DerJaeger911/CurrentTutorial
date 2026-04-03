using Godot;
using Metroidvania.game.interfaces;
using System;

public partial class Drone : CharacterBody2D, IDamageable
{
	private Vector2 direction;
	private float speed = 50f;
	private Player player;
	private int health = 3;

	private Area2D playerDetectionArea;

	public override void _Ready()
	{
		this.playerDetectionArea = this.GetNode<Area2D>("PlayerDetectionArea");

		this.playerDetectionArea.BodyEntered += this.OnPlayerDetected;
		this.playerDetectionArea.BodyExited += this.OnPlayerLost;
	}

	public override void _PhysicsProcess(Double delta)
	{
		if (this.player != null)
		{
			var directionToPlayer = (this.player.Position - this.Position).Normalized();
			this.Velocity = directionToPlayer * this.speed;
			
			this.MoveAndSlide();
		}
	}

	private void OnPlayerDetected(Node body)
	{
		if (body is Player player)
		{
			this.player = player;
		}
	}

	private void OnPlayerLost(Node body)
	{
		if (body is Player player && this.player == player)
		{
			this.player = null;
		}
	}

	public override void _ExitTree()
	{
		this.playerDetectionArea.BodyEntered -= this.OnPlayerDetected;
		this.playerDetectionArea.BodyExited -= this.OnPlayerLost;
	}

	public void Hit()
	{
		this.health--;
	}
}
