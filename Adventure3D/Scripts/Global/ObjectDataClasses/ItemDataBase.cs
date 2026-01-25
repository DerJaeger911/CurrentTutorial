using Godot;

namespace Adventuregame.Scripts.Global.ObjectDataClasses;

public partial class ItemDataBase : RefCounted
{
	public string Type { get; set; } = "item";
	public Texture2D Thumbnail { get; set; }
	public PackedScene Scene { get; set; }
}
