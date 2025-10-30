using Godot;
using System;

namespace BMIapp.Scripts;

public partial class Bmi : Control
{

    private LineEdit heightEntry;
    private HSlider heightSlider;
    private LineEdit weighgEntry;
    private Label bmiOutput;

    private int height = 196;
    private int weight = 105;
    private decimal bmiValue;

    public int Height 
    { 
        get => this.height;

        set 
        { 
            this.height = value;
            this.CalculateBmi();
            GD.Print($"Height = {this.height}");
        } 
    }

    public decimal BmiValue 
    { 
        get => this.bmiValue;

        set
        {
            this.bmiValue = value;
            GD.Print($"BMI = {this.bmiValue}");
        } 
    }

    public override void _Ready()
    {
        this.heightEntry = this.GetNode<LineEdit>("VBoxContainer/HeightInput/PanelContainer/MarginContainer/VBoxContainer/LineEdit");
        this.heightSlider = this.GetNode<HSlider>("VBoxContainer/HeightInput/PanelContainer/MarginContainer/VBoxContainer/Control/HSlider");
        this.weighgEntry = this.GetNode<LineEdit>("VBoxContainer/WeightInput/PanelContainer/HBoxContainer/LineEdit");
        this.bmiOutput = this.GetNode<Label>("VBoxContainer/Output/Label");

        this.heightEntry.Text = this.Height.ToString();
        this.heightSlider.Value = this.Height;
        this.weighgEntry.Text = this.weight.ToString();
        this.heightSlider.ValueChanged += this.OnHeightSliderValueChanged;

        this.CalculateBmi();
    }

    private void OnHeightSliderValueChanged(double value)
    {
        this.Height = (int)value;
    }

    private void CalculateBmi()
    {
        this.BmiValue = (decimal)this.weight / ((decimal)Mathf.Pow(this.Height, 2) * 0.01m);
        this.bmiOutput.Text = DecimalSnapped(this.BmiValue, 0.1m).ToString();
    }

    public override void _ExitTree()
    {
        this.heightSlider.ValueChanged -= this.OnHeightSliderValueChanged;
    }
    //HELPERS
    private static decimal DecimalSnapped(decimal value, decimal step)
    {
        return Math.Round(value / step) * step;
    }
}
