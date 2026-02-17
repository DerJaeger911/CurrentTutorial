using Adventuregame.Scripts.Characters.Player;
using Adventuregame.Scripts.Items;
using Godot;
using System;
using System.Collections.Generic;
using System.Transactions;

public partial class TransitionGate : Area3D
{
	private CollisionShape3D collisionShape;
	private TransitionLayer transitionLayer;

	private Dictionary<LevelEnum, string> levelScenePaths = new Dictionary<LevelEnum, string>()
	{
		{ LevelEnum.OverWorld, "res://scenes/levels/overworld.tscn" },
		{ LevelEnum.Castle, "res://scenes/levels/castle.tscn" },
	};

	[Export]
	private LevelEnum targetLevel = LevelEnum.OverWorld;

	override public void _Ready()
	{
		this.transitionLayer = this.GetNode<TransitionLayer>("/root/TransitionLayer");
		this.collisionShape = this.GetNode<CollisionShape3D>("CollisionShape3D");
		this.BodyEntered += this.OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is Player player)
		{
			this.transitionLayer.ChangeScene(this.levelScenePaths[this.targetLevel]);
		}
	}

	public override void _ExitTree()
	{
		this.BodyEntered -= this.OnBodyEntered;
	}
}
