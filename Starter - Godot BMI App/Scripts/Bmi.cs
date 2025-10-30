using Godot;
using System;
using System.Linq;

namespace BMIapp.Scripts;

public partial class Bmi : Control
{

    private LineEdit heightEntry;
    private HSlider heightSlider;
    private LineEdit weightEntry;
    private Label bmiOutput;
    private Button minusButton;
    private Button plusButton;

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
        } 
    }

    public int Weight 
    { 
        get => this.weight;

        set
        {
            this.weight = value;
            this.CalculateBmi();
        }
    }

    public decimal BmiValue 
    { 
        get => this.bmiValue;

        set
        {
            this.bmiValue = value;
        } 
    }

    public override void _Ready()
    {
        this.heightEntry = this.GetNode<LineEdit>("VBoxContainer/HeightInput/PanelContainer/MarginContainer/VBoxContainer/HeightLineEdit");
        this.heightSlider = this.GetNode<HSlider>("VBoxContainer/HeightInput/PanelContainer/MarginContainer/VBoxContainer/Control/HSlider");
        this.weightEntry = this.GetNode<LineEdit>("VBoxContainer/WeightInput/PanelContainer/HBoxContainer/WeightLineEdit");
        this.bmiOutput = this.GetNode<Label>("VBoxContainer/Output/Label");

        this.minusButton = this.GetNode<Button>("VBoxContainer/WeightInput/PanelContainer/HBoxContainer/MinusButton");
        this.plusButton = this.GetNode<Button>("VBoxContainer/WeightInput/PanelContainer/HBoxContainer/PlusButton");

        this.heightEntry.Text = this.Height.ToString();
        this.heightSlider.Value = this.Height;
        this.weightEntry.Text = this.Weight.ToString();

        this.heightEntry.TextChanged += this.OnHeightEntryChanged;
        this.weightEntry.TextChanged += this.OnWeightEntryChanged;
        this.heightSlider.ValueChanged += this.OnHeightSliderValueChanged;
        this.minusButton.Pressed += this.OnMinusButtonPressed;
        this.plusButton.Pressed += this.OnPlusButtonPressed;

        this.CalculateBmi();
    }

    private void OnHeightSliderValueChanged(double value)
    {
        this.Height = (int)value;
        this.heightEntry.Text = this.Height.ToString();
    }

    private void OnMinusButtonPressed()
    {
        this.Weight -= 1;
        this.weightEntry.Text = this.Weight.ToString();
    }

    private void OnPlusButtonPressed()
    {
        this.Weight += 1;
        this.weightEntry.Text = this.Weight.ToString();
    }

    private void OnHeightEntryChanged(string value)
    {
        int oldCaret = this.heightEntry.CaretColumn;

        string digitsOnly = string.Concat(value.Where(c => char.IsDigit(c)));

        int removedBeforeCaret = value.Substring(0, oldCaret).Count(c => !char.IsDigit(c));
        int newCaret = oldCaret - removedBeforeCaret;
        newCaret = Math.Clamp(newCaret, 0, digitsOnly.Length);

        this.heightEntry.Text = digitsOnly;
        this.heightEntry.CaretColumn = newCaret;

        if (int.TryParse(digitsOnly, out int newHeight))
        {
            newHeight = Math.Clamp(newHeight, 100, 200);

            if (newHeight != this.Height)
            {
                this.Height = newHeight;

                this.heightSlider.ValueChanged -= this.OnHeightSliderValueChanged;
                this.heightSlider.Value = this.Height;
                this.heightSlider.ValueChanged += this.OnHeightSliderValueChanged;
            }
        }
    }

    private void OnWeightEntryChanged (string value)
    {
        int oldCaret = this.weightEntry.CaretColumn;

        string digitsOnly = string.Concat(value.Where(c => char.IsDigit(c)));

        int removedBeforeCaret = value.Substring(0, oldCaret).Count(c => !char.IsDigit(c));
        int newCaret = oldCaret - removedBeforeCaret;
        newCaret = Math.Clamp(newCaret, 0, digitsOnly.Length);

        this.weightEntry.Text = digitsOnly;
        this.weightEntry.CaretColumn = newCaret;

        if (int.TryParse(digitsOnly, out int newWeight))
        {
            if (newWeight != this.Weight)
            {
                this.Weight = newWeight;
            }
        }
    }

    private void CalculateBmi()
    {
        this.BmiValue = (decimal)this.Weight / ((decimal)Mathf.Pow(this.Height, 2) / 100) * 100;
        this.bmiOutput.Text = Helpers.DecimalSnapped(this.BmiValue, 0.1m).ToString();
    }

    public override void _ExitTree()
    {
        this.heightSlider.ValueChanged -= this.OnHeightSliderValueChanged;
        this.minusButton.Pressed -= this.OnMinusButtonPressed;
        this.plusButton.Pressed -= this.OnPlusButtonPressed;
    }
}
