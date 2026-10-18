using Godot;
using System;

public partial class NeedBar : ProgressBar
{
    [Export]
    private Label text;
    [Export]
    private NeedNameEnums needName;

    public override void _Ready()
    {
        this.UpdateValue(200,500);
    }

    public void UpdateValue(float newValue, float max)
    {
        this.Value = newValue;
        this.MaxValue = max;
        this.text.Text = $"{this.needName} : {Mathf.RoundToInt(this.Value)} / {Mathf.RoundToInt(this.MaxValue)}";
    }
}
