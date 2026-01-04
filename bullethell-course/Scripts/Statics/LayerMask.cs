using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethellcourse.Scripts.Statics;

internal static class LayerMask
{
	public static uint BaseLayer = 1 << 0;
	public static uint PlayerLayer = 1 << 1;
	public static uint EnemyLayer = 1 << 2;
	public static uint ProjectileLayer = 1 << 3;
	public static uint ItemLayer = 1 << 4;

	public static uint NoCollision = 0;

	public static uint EntityMask = BaseLayer | ProjectileLayer;
	public static uint PlayerMask = BaseLayer | ProjectileLayer | ItemLayer;
	public static uint ItemMask = PlayerLayer;
	public static uint ProjectileMask = BaseLayer | PlayerLayer | EnemyLayer;

	public static uint DisableMasks(uint currentMask, uint layerToDisable)
	{
		currentMask &= ~(layerToDisable);
		return currentMask;
	}

	public static uint AddLayer(uint currentMask, uint layerToAdd)
	{
		currentMask |= layerToAdd;
		return currentMask;
	}
}
