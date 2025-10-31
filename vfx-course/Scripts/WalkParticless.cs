using Godot;
using System;

public partial class WalkParticless : GpuParticles3D
{
    private CharacterBody3D character;
    private float vectorOffset = 0.1f;
    private int isTrue = 0;

    public override void _Ready()
    {
        this.character = this.GetParent<CharacterBody3D>();
    }

    public override void _Process(double delta)
    {
        this.Emitting = this.CanEmit();
    }

    private bool CanEmit()
    {
        if (this.character.Velocity.Length() > this.vectorOffset && this.character.IsOnFloor())
        {
            return true;
        }
        
        return false;
    }

}
