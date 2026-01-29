using Godot;

namespace Adventuregame.Scripts.GlobalData.ObjectDataClasses;

public partial class ItemDataBase : RefCounted
{
	public string Type { get; set; } = "item";
	public Texture2D Thumbnail { get; set; }
	public PackedScene Scene { get; set; }
}
