using Godot;

namespace RoguelikeCourse.Scripts.Manager.PreloadManagers.AssetPreloads
{
    internal class AssetPreloads
    {
        private static AssetPreloads instance = new();
        public static AssetPreloads Instance => instance;

        private static PackedScene playerScene = GD.Load<PackedScene>("res://Scenes/Entities/player.tscn");

        private static PackedScene crosshairScene = GD.Load<PackedScene>("res://Scenes/crosshair.tscn");

        public static Player Player => playerScene.Instantiate() as Player;

        public static Node2D Crosshair => crosshairScene.Instantiate() as Node2D;

    }
}
