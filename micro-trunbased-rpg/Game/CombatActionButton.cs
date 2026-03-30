using Godot;
using System;

namespace MicroTrunbasedRPG.Game;

public partial class CombatActionButton : Button
{
	public CombatAction Action { get; set; }

	public void SetCombatAction(CombatAction combatAction)
	{
		this.Action = combatAction;
		this.Text = combatAction.DisplayName;
	}
}
