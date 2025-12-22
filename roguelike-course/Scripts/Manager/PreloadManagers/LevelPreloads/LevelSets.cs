using Godot.Collections;

namespace RoguelikeCourse.Scripts.Manager.PreloadManagers.LevelPreloads
{
    internal static class LevelSets
    {
        public static Array<LevelEnum> InnerRooms { get; } = new()
        {
            LevelEnum.Enemy1,
            LevelEnum.Enemy2,
            LevelEnum.Enemy3,
            LevelEnum.Enemy4,
            LevelEnum.Item
        };
    }
}
