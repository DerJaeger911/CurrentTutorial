using System;

namespace twentyfourtyeight.src.main.SignalHubs;

public class UISignals
{
	public static event Action OnEndTurn;
	public static void EmitOnEndTurn() => OnEndTurn?.Invoke();

	public static event Action<Unit> UnitClicked;
	public static void EmitUnitClicked(Unit unit) => UnitClicked?.Invoke(unit);
}