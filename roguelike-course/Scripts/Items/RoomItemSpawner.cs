using Godot;
using RoguelikeCourse.Scripts.Manager.PreloadManagers.ItemPreloads;
using System.Linq;

public partial class RoomItemSpawner : Node2D
{
    public override void _Ready()
    {
		var item = ItemPreloads.Instance.GetRandomItem(ItemSets.Potions.ToArray());
		this.CallDeferred(Node.MethodName.AddChild, item);
	}
}