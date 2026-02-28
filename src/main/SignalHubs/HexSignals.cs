using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twentyfourtyeight.src.main.SignalHubs;

public class HexSignals
{
	public static event Action<Hex> OnSendHexData;
	public static void EmitSendHexData(Hex hex) => OnSendHexData?.Invoke(hex);

	public static event Action OnClickOffMap;
	public static void EmitClickOffMap() => OnClickOffMap?.Invoke();
}
