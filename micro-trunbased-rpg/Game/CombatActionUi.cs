using Godot;
using Godot.Collections;
using System;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;

namespace MicroTrunbasedRPG.Game;

public partial class CombatActionUi : Panel
{
	[Export]
	private Button passTurnButton;
	private VBoxContainer actionContainer;
	private Array<CombatActionButton> combatActionButtons = new();
	private RichTextLabel descriptionLabel;
	private GameManager gameManager;

	override public void _Ready()
	{
		this.actionContainer = this.GetNode<VBoxContainer>("MarginContainer/HBoxContainer/ActionContainer");
		this.descriptionLabel = this.GetNode<RichTextLabel>("MarginContainer/HBoxContainer/RichTextLabel");
		this.gameManager = this.GetNode<GameManager>("../..");

		foreach(var child in this.actionContainer.GetChildren())
		{
			if (child is not CombatActionButton button)
			{
				continue;
			}

			this.combatActionButtons.Add(button);
			button.Pressed += () => this.ButtonPressed(button);
			button.MouseEntered += () => this.ButtonEntered(button);
			button.MouseExited += () => this.ButtonExited(button);
		}

		this.passTurnButton.Pressed += this.OnPassTurnButtonPressed;

	}

	public void SetCombatActions(Array<CombatAction> combatActions)
	{
		for(int i = 0; i < this.combatActionButtons.Count; i++)
		{
			if (i >= combatActions.Count)
			{
				this.combatActionButtons[i].Visible = false;
				continue;
			}
			this.combatActionButtons[i].Visible = true;
			this.combatActionButtons[i].SetCombatAction(combatActions[i]);
		}
	}

	private void ButtonPressed(CombatActionButton button)
	{
		_= this.gameManager.PlayerCastCombatAction(button.Action);
	}

	private void ButtonEntered(CombatActionButton button)
	{
		var action = button.Action;
		this.descriptionLabel.Text = $"[b]{action.DisplayName}[/b]\n{action.Description}";
	}

	private void ButtonExited(CombatActionButton button)
	{
		this.descriptionLabel.Text = "";
	}

	private void OnPassTurnButtonPressed()
	{
		_ = this.gameManager.NextTurn();
	}

	public override void _ExitTree()
	{
		foreach (var button in this.combatActionButtons)
		{
			if (IsInstanceValid(button))
			{
				button.Pressed -= () => this.ButtonPressed(button);
				button.MouseEntered -= () => this.ButtonEntered(button);
				button.MouseExited -= () => this.ButtonExited(button);
			}
		}
		this.passTurnButton.Pressed -= this.OnPassTurnButtonPressed;
		this.combatActionButtons.Clear();
	}
}
