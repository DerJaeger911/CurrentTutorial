using System;

namespace twentyfourtyeight.src.main.SignalHubs;

public class UISignals
{
	public static event Action OnEndTurn;
	public static void EmitOnEndTurn() => OnEndTurn?.Invoke();
}
