using Godot;

namespace Adventuregame.Scripts.GlobalData.Preloads;

public static class PreloadScenes
{
	public static readonly PackedScene HeartScene = GD.Load<PackedScene>("res://scenes/ui/heart.tscn");
	public static readonly PackedScene ItemScene = GD.Load<PackedScene>("res://scenes/ui/item.tscn");
}
