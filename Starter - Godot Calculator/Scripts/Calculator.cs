using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorApp.Scripts
{
    public partial class Calculator : Control
    {
        private Button zeroButton;
        private Label label;

        private readonly Dictionary<Button, Action> buttonHandlers = [];

        private Action zeroButtonHandler;
        private Action resizeHandler;

        private readonly HashSet<string> operators = ["+", "-", "/", "x"];

        private List<string> tempNums = [];
        private readonly List<string> finNums = [];

        public override void _Ready()
        {
            this.zeroButton = this.GetNode<Button>("VBoxContainer/PanelContainer/GridContainer/ZeroButtonContainer/ZeroButton");
            this.label = this.GetNode<Label>("VBoxContainer/MarginContainer/Label");

            this.resizeHandler = () => this.zeroButton.OffsetLeft = -this.GetWindow().Size.X / 4;
            this.zeroButton.Resized += this.resizeHandler;


            this.zeroButtonHandler = () => this.OnButtonPressed(this.zeroButton.Text);
            this.zeroButton.Pressed += this.zeroButtonHandler;
            this.buttonHandlers[this.zeroButton] = this.zeroButtonHandler;

            this.label.Text = string.Empty;

            foreach (Node child in this.GetNode<GridContainer>("VBoxContainer/PanelContainer/GridContainer").GetChildren())
            {
                if (child is Button button)
                {
                    void handler() => this.OnButtonPressed(button.Text);
                    button.Pressed += handler;

                    this.buttonHandlers[button] = handler;
                }
            }
        }

        private void OnButtonPressed(string character)
        {
            if (character.IsValidInt())
            {
                this.tempNums.Add(character);

                this.label.Text = this.ListToString(this.tempNums);
            }
            
            if (this.operators.Contains(character))
            {
                if (!string.IsNullOrEmpty(this.label.Text))
                {
                    this.finNums.Add(this.label.Text);
                    this.ClearTemp();
                    this.ClearScreen();
                    this.finNums.Add(character != "x" ? character : "*");
                }
            }

            switch (character)
            {
                case "=": 
                    Expression expression = new();
                    if (!string.IsNullOrEmpty(this.label.Text))
                    {
                        this.finNums.Add(this.label.Text);
                    }

                    var equation = this.ListToString(this.finNums);

                    if (equation.Length > 0 && this.operators.Contains(equation.Last().ToString()))
                    {
                        equation = equation.Substring(0, equation.Length - 1);
                    }

                    expression.Parse(equation);

                    this.label.Text = expression.Execute().ToString();
                    this.ClearTemp();
                    this.ClearFin();
                    this.tempNums.Add(this.label.Text);
                    break;

                case ".": 
                    if (!this.tempNums.Contains("."))
                    this.tempNums.Add(".");
                    this.label.Text = this.ListToString(this.tempNums);
                    break;

                case "%":
                    if (this.tempNums.Count == 0) break;

                    decimal number = Convert.ToDecimal(this.ListToString(this.tempNums));
                    number *= 0.01m;

                    this.ClearTemp();
                    this.tempNums.Add(number.ToString("G")); 
                    this.label.Text = this.ListToString(this.tempNums);
                    GD.Print(this.label.Text);
                    break;

                case "+/-": 
                    if (this.tempNums[0] == "-")
                    {
                        this.tempNums.RemoveAt(0);
                    }
                    else
                    {
                        this.tempNums.Insert(0,  "-");
                    }
                    this.label.Text = this.ListToString(this.tempNums);
                    break;

                case "C":
                    this.ClearTemp();
                    this.ClearScreen();
                    break;
            }
        }

        private void ClearTemp()
        {
            this.tempNums.Clear();
            
        }

        private void ClearScreen()
        {
            this.label.Text = string.Empty;
        }

        private void ClearFin()
        {
            this.finNums.Clear();
        }

        private string ListToString(List<string> characters)
        {
            var s = string.Empty;
            for (int i = 0; i < characters.Count; i++)
            {
                s += characters[i];
            }
            return s;
        }

        public override void _ExitTree()
        {
            this.zeroButton.Resized -= this.resizeHandler;

            foreach (var pair in this.buttonHandlers)
            {
                pair.Key.Pressed -= pair.Value;
            }

            this.buttonHandlers.Clear();
        }
    }
}
