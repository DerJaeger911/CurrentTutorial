using Godot;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Interfaces;
using System;

public partial class Projectile : Area2D
{
    [Export]
    private float baseSpeed = 100;

    private CharacterBody2D ownerCharacter;

    public override void _Ready()
    {
        this.BodyEntered += this.OnBodyEntered;
    }

    public override void _Process(double delta)
    {
        this.Translate( - this.Transform.Y * this.baseSpeed * (float)delta);
    }

    private void OnBodyEntered(Node body)
    {
        if (body == this.ownerCharacter)
        {
            return;
        }

        if (body is IEntity entity)
        {
            entity.TakeDamage(1);

            this.QueueFree();
        }
    }
}
