using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.PhysicsLayers;

internal class LayerMask
{
	public static uint BaseLayer = 1 << 0;
	public static uint TerainLayer = 1 << 1;
	public static uint PlayerLayer = 1 << 2;
	public static uint PlantLayer = 1 << 3;
	public static uint EnemyLayer = 1 << 4;

	public static uint NoCollision = 0;

	public static uint EnemyMask = BaseLayer;
	public static uint PlayerMask = BaseLayer;
	public static uint TerainMask = BaseLayer;
	public static uint PlantMask = BaseLayer;


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
