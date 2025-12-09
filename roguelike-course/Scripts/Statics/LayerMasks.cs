namespace RoguelikeCourse.Scripts.Statics
{
    internal static class LayerMasks
    {
        public static uint BaseLayer = 1 << 0;
        public static uint PlayerLayer = 1 << 1;
        public static uint EnemyLayer = 1 << 3;
        public static uint ProjectileLayer = 1 << 4;

        public static uint PlayerMasks = BaseLayer | ProjectileLayer;
        public static uint EnemyMasks = BaseLayer | ProjectileLayer;
    }
}
