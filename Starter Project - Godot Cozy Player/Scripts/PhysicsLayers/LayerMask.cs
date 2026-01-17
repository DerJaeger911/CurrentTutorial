using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.PhysicsLayers;

internal class LayerMask
{
	public const uint BaseLayer = 1 << 0;
	public const uint TerainLayer = 1 << 1;
	public const uint PlayerLayer = 1 << 3;
	public const uint PlantLayer = 1 << 4;
	public const uint EnemyLayer = 1 << 5;

	public const uint NoCollision = 0;

	public static uint EnemyMask = BaseLayer | TerainLayer;
	public static uint PlayerMask = BaseLayer | TerainLayer;
	public static uint TerainMask = BaseLayer;
	public static uint PlantMask = BaseLayer;
	public static uint EnemyAttackMask = PlayerLayer;


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
