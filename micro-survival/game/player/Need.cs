using Godot;
using System;

public partial class Need : Control
{
    [Export]
    public float MaxValue {get; set;}
    [Export]
    public float StartValue {get; set;}
    [Export]
    public float RegenRate{get; set;}
    [Export]
    public float DecayRate {get; set;}
    [Export]
    private NeedBar uiBar;

    public float Value {get; set;}

    public void Add(float amount)
    {
        this.Value += amount;

        if (this.Value > this.MaxValue)
        {
            this.Value = this.MaxValue;
        }
    }

    public void Subtract(float amount)
    {
        this.Value -= amount;

        if (this.Value < 0)
        {
            this.Value = 0;
        }
    }

    public void UpdateUiBar()
    {
        this.uiBar.UpdateValue(this.Value, this.MaxValue);
    }
}
