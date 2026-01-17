using Dcozysandbox.Scripts.Enemies;
using Dcozysandbox.Scripts.Entities.Player;
using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class SomeCollider : Area2D
{
	private CollisionShape2D collision;
    private Enemy self;

    public override void _Ready()
    {
        this.collision = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.CollisionMask = LayerMask.EnemyAttackMask;
        this.self = this.GetParent<Enemy>();
        this.BodyEntered += this.OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if(body is Player)
        {
            this.self.Die();
        }
    }
}
