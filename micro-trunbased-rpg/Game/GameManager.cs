using Godot;
using Godot.Collections;
using System.Threading.Tasks;

namespace MicroTrunbasedRPG.Game;

public partial class GameManager : Node2D
{
	[Export]
	private Character playerCharacter;
	[Export]
	private Character aiCharacter;

	private Character currentCharacter;

	private bool gameOver;

	private CombatActionUi playerUi;

	override public void _Ready()
	{
		this.playerUi = this.GetNode<CombatActionUi>("CanvasLayer/CombatActionUi");
		this.gameOver = false;
		_ = this.NextTurn();
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}

	public async Task NextTurn()
	{
		if (this.gameOver)
		{
			return;
		}

		if (this.currentCharacter != null)
		{
			this.currentCharacter.EndTurn();
		}

		if (this.currentCharacter == this.aiCharacter || this.currentCharacter == null)
		{
			this.currentCharacter = this.playerCharacter;
		}
		else
		{
			this.currentCharacter = this.aiCharacter;
		}

		this.currentCharacter.BeginTurn();

		if (this.currentCharacter == this.playerCharacter)
		{
			this.playerUi.Visible = true;
			
			this.playerUi.SetCombatActions(this.playerCharacter.CombatActions);
		}
		else
		{
			this.playerUi.Visible = false;
			var waitTime = GD.RandRange(0.5f, 1.5f);
			await this.ToSignal(this.GetTree().CreateTimer(waitTime), SceneTreeTimer.SignalName.Timeout);
			CombatAction action = this.AiDecideCombatAction();
			this.aiCharacter.CastCombatAction(action, this.playerCharacter);

			await this.ToSignal(this.GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
			this.CheckForWinner();
			_ = this.NextTurn();
		}
	}

	public async Task PlayerCastCombatAction(CombatAction action)
	{
		if (this.currentCharacter != this.playerCharacter)
		{
			return;
		}

		this.playerCharacter.CastCombatAction(action, this.aiCharacter);
		this.playerUi.Visible = false;
		await this.ToSignal(this.GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
		this.CheckForWinner();
		_ = this.NextTurn();
	}

	private CombatAction AiDecideCombatAction()
	{
		if(this.currentCharacter != this.aiCharacter)
		{
			return null;
		}

		Array<int> weights = new();

		int totalWeight = 0;

		float aiHealthPercent = (float)this.aiCharacter.CurrentHealth / (float)this.aiCharacter.MaxHealth;

		float playerHealthPercent = (float)this.playerCharacter.CurrentHealth / (float)this.playerCharacter.MaxHealth;

		foreach(CombatAction action in this.aiCharacter.CombatActions)
		{
			float weight = action.BaseWeight;

			if (this.playerCharacter.CurrentHealth <= action.MeleeDamage)
			{
				weight *= 3;
			}
			if (action.HealAmount > 0)
			{
				weight *= 1 + (1 - aiHealthPercent);
			}
			weights.Add((int)weight);
			totalWeight += (int)weight;
		}

		int cumulativeWeight = 0;
		int randWeight = GD.RandRange(0, totalWeight);

		for(int i = 0; i < this.aiCharacter.CombatActions.Count; i++)
		{
			cumulativeWeight += weights[i];

			if (randWeight < cumulativeWeight)
			{
				return this.aiCharacter.CombatActions[i];
			}
		}

		return null;
	}

	private void CheckForWinner()
	{
		if (this.playerCharacter.CurrentHealth <= 0)
		{
			this.gameOver = true;
			SignalHub.Instance.EmitGameLoser(this.playerCharacter);
		}
		else if (this.aiCharacter.CurrentHealth <= 0)
		{
			this.gameOver = true;
			SignalHub.Instance.EmitGameLoser(this.aiCharacter);
		}
	}
}
