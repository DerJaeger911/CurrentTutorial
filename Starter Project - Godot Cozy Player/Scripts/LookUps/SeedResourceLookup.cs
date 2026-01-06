using Dcozysandbox.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.LookUps;

internal class SeedResourceLookup
{
	public static readonly IReadOnlyDictionary<SeedEnum, ResourceEnum> ResourceSeedConnection =
		new Dictionary<SeedEnum, ResourceEnum>
		{
		{ SeedEnum.Corn, ResourceEnum.Corn },
		{ SeedEnum.Tomato, ResourceEnum.Tomato },
		{ SeedEnum.Pumpkin, ResourceEnum.Pumpkin }
		}.AsReadOnly();
}
