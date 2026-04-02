using Godot;
using MicroTrunbasedRPG.Game;
using System;

public partial class EndScreen : Panel
{
	private Button retryButton;
	private Label endScreenLabel;

	public override void _Ready()
	{
		this.retryButton = this.GetNode<Button>("VBoxContainer/Button");
		this.endScreenLabel = this.GetNode<Label>("VBoxContainer/Label");

		SignalHub.Instance.GameLoser += this.ChangeLabelText;
		this.retryButton.Pressed += this.OnRetryButtonPressed;

		this.Visible = false;
	}

	private void ChangeLabelText(Character character)
	{
		this.Visible = true;
		if (!character.IsPlayer ) 
		{
			this.endScreenLabel.Text = "You have won!";
		}
		else
		{
			this.endScreenLabel.Text = "You have lost!";
		}
	}

	private void OnRetryButtonPressed()
	{
		this.GetTree().ReloadCurrentScene();
		this.Visible = false;
	}

	public override void _ExitTree()
	{
		SignalHub.Instance.GameLoser -= this.ChangeLabelText;
	}
}
