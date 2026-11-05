using Godot;
using System;

public partial class WorldItem : InteractableObject
{
	[Export]
	private string itemName;

	public override void Interact()
	{
		Item item = (Item)GD.Load("res://game/items/itemData/" + this.itemName + ".tres");
		SignalHub.Instance.EmitGivePlayerItem(item, 1);
		this.QueueFree();
	}
}
