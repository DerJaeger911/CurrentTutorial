using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;

namespace Dcozysandbox.Scripts.Enemies;

public partial class Enemy : CharacterBody2D
{

	public override void _Ready()
	{
		base._Ready();
		this.CollisionLayer = LayerMask.EnemyLayer;
		this.CollisionMask = LayerMask.EnemyMask;
	}
}
