using System;

namespace twentyfourtyeight.src.main.SignalHubs;

public class UISignals
{
	public static event Action OnEndTurn;
	public static void EmitOnEndTurn() => OnEndTurn?.Invoke();

	public static event Action<Unit> OnUnitClicked;
	public static void EmitUnitClicked(Unit unit) => OnUnitClicked?.Invoke(unit);

    public static event Action<Hex> OnRightClickOnMapEventHandler;
    public static void EmitRightClickOnMapEventHandler(Hex hex) => OnRightClickOnMapEventHandler?.Invoke(hex);

    public static event Action OnSelectedUnitDestroyedEventHandler;
    public static void EmitSelectedUnitDestroyedEventHandler() => OnSelectedUnitDestroyedEventHandler?.Invoke();
}