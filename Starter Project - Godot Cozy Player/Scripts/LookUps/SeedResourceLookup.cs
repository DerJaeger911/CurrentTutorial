using Dcozysandbox.Scripts.Enums;
using System.Collections.Generic;

namespace Dcozysandbox.Scripts.LookUps;

internal static class SeedResourceLookup
{
	public static readonly IReadOnlyDictionary<SeedEnum, ResourceEnum> ResourceSeedConnection =
		new Dictionary<SeedEnum, ResourceEnum>
		{
		{ SeedEnum.Corn, ResourceEnum.Corn },
		{ SeedEnum.Tomato, ResourceEnum.Tomato },
		{ SeedEnum.Pumpkin, ResourceEnum.Pumpkin }
		}.AsReadOnly();
}
