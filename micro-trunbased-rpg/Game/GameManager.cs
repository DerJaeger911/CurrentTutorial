using Godot;
using System.Runtime.CompilerServices;
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

	override public void _Ready()
	{
		_ = this.NextTurn();
	}

	private async Task NextTurn()
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
			_ = this.NextTurn();
		}
		else
		{
			var waitTime = GD.RandRange(0.5f, 1.5f);
			await this.ToSignal(this.GetTree().CreateTimer(waitTime), SceneTreeTimer.SignalName.Timeout);
			await this.ToSignal(this.GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);

			_ = this.NextTurn();
		}
	}

	private async Task PlayerCastCombatAction(CombatAction action)
	{
		if (this.currentCharacter != this.playerCharacter)
		{
			return;
		}

		this.playerCharacter.CastCombatAction(action, this.aiCharacter);

		await this.ToSignal(this.GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);

		_ = this.NextTurn();
	}

	private void AiDecideCombatAction()
	{

	}
}
