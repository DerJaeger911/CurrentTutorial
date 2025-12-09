using Godot;
using RoguelikeCourse.Scripts.Entities.Bases;

namespace RoguelikeCourse.Scripts;

public partial class Projectile : Area2D
{
    [Export]
    private float baseSpeed = 100;

    public CharacterBody2D ownerCharacter;

    private Timer autoDestroytimer;

    public override void _Ready()
    {
        this.autoDestroytimer = this.GetNode<Timer>("Timer");
        this.autoDestroytimer.Timeout += this.OnTimeOut;

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

        if (body is Entity entity)
        {
            entity.TakeDamage(1);
        }
        this.QueueFree();
    }

    private void OnTimeOut()
    {
        this.QueueFree();
    }

    public override void _ExitTree()
    {
        this.autoDestroytimer.Timeout -= this.OnTimeOut;
        this.BodyEntered -= this.OnBodyEntered;
    }
}
