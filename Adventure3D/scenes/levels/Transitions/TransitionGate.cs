using Adventuregame.Scripts.Characters.Player;
using Godot;
using System;

public partial class TransitionGate : Area3D
{
	private CollisionShape3D collisionShape;

	override public void _Ready()
	{
		this.collisionShape = this.GetNode<CollisionShape3D>("CollisionShape3D");
		this.BodyEntered += this.OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is Player player)
		{
		}
	}

	public override void _ExitTree()
	{
		this.BodyEntered -= this.OnBodyEntered;
	}
}
