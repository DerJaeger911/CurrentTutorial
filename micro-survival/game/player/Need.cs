using Godot;
using System;

public partial class Need : Control
{
    [Export]
    private float maxValue;
    [Export]
    private float startValue;
    [Export]
    private float regenRate;
    [Export]
    private float decayRate;
    [Export]
    private NeedBar uiBar;

    private float value;

    private void Add(float amount)
    {
        this.value += amount;

        if (this.value > this.maxValue)
        {
            this.value = this.maxValue;
        }
    }

    private void Subtract(float amount)
    {
        this.value -= amount;

        if (this.value < 0)
        {
            this.value = 0;
        }
    }

    public void UpdateUiBar()
    {
        this.uiBar.UpdateValue(this.value, this.maxValue);
    }
}
