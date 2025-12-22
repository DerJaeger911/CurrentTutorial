namespace RoguelikeCourse.Scripts.Statics
{
    internal static class LayerMasks
    {
        public static uint BaseLayer = 1 << 0;
        public static uint PlayerLayer = 1 << 1;
        public static uint EnemyLayer = 1 << 2;
        public static uint ProjectileLayer = 1 << 3;

        public static uint NoCollision = 0;

        public static uint EntityMask = BaseLayer | ProjectileLayer;
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
}
