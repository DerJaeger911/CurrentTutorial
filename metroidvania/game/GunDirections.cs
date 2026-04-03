using Godot;
using System.Collections.Generic;

namespace Metroidvania.game;

public class GunDirections
{
	public static readonly Dictionary<Vector2I, int> GunDirectionDict = new()
	{
		{ new Vector2I(0, 0), 0 },
		{ new Vector2I(1, 0), 0 },
		{ new Vector2I(1, 1), 1 },
		{ new Vector2I(0, 1), 2 },
		{ new Vector2I(-1, 1), 3 },
		{ new Vector2I(-1, 0), 4 },
		{ new Vector2I(-1, -1), 5 },
		{ new Vector2I(0, -1), 6 },
		{ new Vector2I(1, -1), 7 }
	};
}
