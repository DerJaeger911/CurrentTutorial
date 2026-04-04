using Godot;
using Metroidvania.game.interfaces;
using Metroidvania.game.utility;
using System;
using System.Diagnostics;

public partial class Drone : CharacterBody2D, IDamageable
{
	private Vector2 direction;
	private float speed = 50f;
	private Player player;
	private int health = 3;
	private bool isDying;

	private Area2D playerDetectionArea;
	private Area2D explosionDetectionArea;
	private AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		this.playerDetectionArea = this.GetNode<Area2D>("PlayerDetectionArea");
		this.animatedSprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.explosionDetectionArea = this.GetNode<Area2D>("ExplosionDetectionArea");

		this.playerDetectionArea.BodyEntered += this.OnPlayerDetected;
		this.playerDetectionArea.BodyExited += this.OnPlayerLost;
		this.explosionDetectionArea.BodyEntered += this.OnExplosionDetected;
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

	private void OnExplosionDetected(Node body)
	{
		this.Die();
	}

	public void Hit()
	{
		this.health--;

		if (this.health <= 0)
		{
			this.Die();
			return;
		}
		
		var shaderMaterial = this.animatedSprite.Material as ShaderMaterial;

		if (shaderMaterial != null)
		{
			shaderMaterial.SetShaderParameter("ColorParameter", new Color(1, 0, 0));
			Tween tween = this.CreateTween();
			tween.TweenProperty(shaderMaterial, "shader_parameter/Progress", 0.7, 0.1);
			tween.TweenProperty(shaderMaterial, "shader_parameter/Progress", 0.0, 0.2);
		}
		else
		{
			GD.PrintErr($"{this.Name} has no Shadermaterial!");
		}
	}

	private void Die()
	{
		this.isDying = true;
		this.CollisionLayer = 0;
		this.SetPhysicsProcess(false);

		Tween tween = this.CreateTween();
		
		FX.SpawnExplosion(this);

		this.QueueFree();
	}

	public override void _ExitTree()
	{
		this.playerDetectionArea.BodyEntered -= this.OnPlayerDetected;
		this.playerDetectionArea.BodyExited -= this.OnPlayerLost;
		this.explosionDetectionArea.BodyEntered -= this.OnExplosionDetected;
	}
}
