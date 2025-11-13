using Godot;
using RoguelikeCourse.Scripts;
using System;

public partial class Projectile : Area2D
{
    [Export]
    private float baseSpeed = 100;

    private CharacterBody2D ownerCharacter;

    public override void _Process(double delta)
    {
        this.Translate( - this.Transform.Y * this.baseSpeed * (float)delta);
    }

    private void OnBodyEntered(Player body)
    {
        if (body == this.ownerCharacter)
        {
            return;
        }

        if (body.HasMethod("TakeDamage"))
        {
            body.TakeDamage(1);

            this.QueueFree();
        }
    }
}
