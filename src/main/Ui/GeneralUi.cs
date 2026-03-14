using Godot;
using twentyfourtyeight.src.main.SignalHubs;

public partial class GeneralUi : Panel
{
	private int turns = 0;

	private Label turnLabel;

	private Button endTurnButton;

	public override void _Ready()
	{
		this.turnLabel = this.GetNode<Label>("MarginContainer/Label");
		this.endTurnButton = this.GetNode<Button>("MarginContainer/EndTurnButton");
		this.SetTurnText();

		this.endTurnButton.Pressed += this.EndTurn;
	}

	public void EndTurn()
	{
		this.IncrementTurnCounter();
		UISignals.EmitOnEndTurn();
		GD.Print("Turn ended");
	}

	public void IncrementTurnCounter()
	{
		this.turns++;
		this.SetTurnText();
	}

	private void SetTurnText()
	{
		this.turnLabel.Text = $"Turn {this.turns}";
	}
}
