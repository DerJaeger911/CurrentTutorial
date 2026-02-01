using Adventuregame.Scripts.GlobalData.Enums;
using Godot;

namespace Adventuregame.Scripts.GlobalData.ObjectDataClasses;

public partial class ItemDataBase : RefCounted
{
	public ItemTypeEnum Type { get; set; } = ItemTypeEnum.Item;
	public Texture2D Thumbnail { get; set; }
	public PackedScene Scene { get; set; }
}
