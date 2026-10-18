using Godot;
using System;

public partial class PlayerNeeds : Node3D
{
    [Export]
    private Need health;
    [Export]
    private Need hunger;
    [Export]
    private Need thirst;
    [Export]
    private Need sleep;
    [Export]
    private float noHungerHealthDecay;
    [Export]
    private float noThirstHealthDecay;

    public override void _Ready()
    {
        this.health.Value = this.health.StartValue;
        this.hunger.Value = this.hunger.StartValue;
        this.thirst.Value = this.thirst.StartValue;
        this.sleep.Value = this.sleep.StartValue;
    }

    public override void _Process(double delta)
    {
        this.hunger.Subtract(this.hunger.DecayRate * (float)delta);
        this.thirst.Subtract(this.thirst.DecayRate * (float)delta);
        this.sleep.Add(this.sleep.RegenRate * (float)delta);

        if (this.hunger.Value == 0)
        {
            this.health.Subtract(this.noHungerHealthDecay * (float)delta);
        }

        if (this.thirst.Value == 0)
        {
            this.health.Subtract(this.noThirstHealthDecay * (float)delta);
        }

        if (this.health.Value == 0)
        {
            GD.Print("Die");
        }

        this.health.UpdateUiBar();
        this.hunger.UpdateUiBar();
        this.thirst.UpdateUiBar();
        this.sleep.UpdateUiBar();
    }

    public void Eat (float amount)
    {
        this.hunger.Add(amount);
    }
}
