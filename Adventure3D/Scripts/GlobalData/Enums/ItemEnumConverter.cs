using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventuregame.Scripts.GlobalData.Enums;

internal class ItemEnumConverter
{
	public static Enum ConvertItemEnum(AllItemEnum item)
	{
		string itemName = item.ToString();

		foreach (WeaponEnum weaponType in Enum.GetValues(typeof(WeaponEnum)))
		{
			if (weaponType.ToString() == itemName)
			{
				return weaponType;
			}
		}
		foreach (ShieldEnum shieldType in Enum.GetValues(typeof(ShieldEnum)))
		{
			if (shieldType.ToString() == itemName)
			{
				return shieldType;
			}
		}
		foreach (StyleEnum styleType in Enum.GetValues(typeof(StyleEnum)))
		{
			if (styleType.ToString() == itemName)
			{
				return styleType;
			}
		}
		GD.Print("Couldnt find Enum");
		return null;
	}
}
