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

	private CombatActionUi playerUi;

	override public void _Ready()
	{
		this.playerUi = this.GetNode<CombatActionUi>("CanvasLayer/CombatActionUi");
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
			this. aiCharacter.CastCombatAction(action, this.playerCharacter);

			await this.ToSignal(this.GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);

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

		_ = this.NextTurn();
	}

	private CombatAction AiDecideCombatAction()
	{
		return null;
	}
}
