using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
	private List<InventorySlot> slots = new();

	[Export]
	private Panel window;

	[Export]
	private Label infoText;

	[Export]
	private Item[] starterItems;

	public override void _Ready()
	{
		this.ToggleWindow(false);

		foreach (Node child in this.window.GetNode("SlotContainer").GetChildren())
		{
			if (child is InventorySlot inventorySlot)
			{
				slots.Add(inventorySlot);
				inventorySlot.SetItem(null);
				inventorySlot.Inventory = this;
			}
		}
		if (this.starterItems != null)
		{
			foreach (Item item in this.starterItems)
			{

				AddItem(item);
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("inventory"))
		{
			this.ToggleWindow(!this.window.Visible);
		}
	}

	private void ToggleWindow(bool open)
	{
		this.window.Visible = open;
		if (open)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;	
		}
	}

	private void OnGivePlayerItem(Item item, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.AddItem(item);
		}
	}

	private void AddItem(Item item)
	{
		InventorySlot slot = this.GetSlotToAdd(item);

		if (slot == null)
		{
			return;
		}

		if (slot.Item == null)
		{
			slot.SetItem(item);
		}
		else if (slot.Item == item)
		{
			slot.AddItem();
		}
	}

	private void RemoveItem(Item item)
	{
		InventorySlot slot = this.GetSlotToRemove(item);

		if (slot == null || slot.Item == null)
		{
			return;
		}

		slot.RemoveItem();
	}

	private InventorySlot GetSlotToAdd(Item item)
	{
		foreach (InventorySlot slot in slots)
		{
			if (slot.Item == item && slot.Quantity < item.MaxStackSize)
			{
				return slot;
			}
		}

		foreach (InventorySlot slot in slots)
		{
			if (slot.Item == null)
			{
				return slot;
			}
		}

		return null;
	}

	private InventorySlot GetSlotToRemove(Item item)
	{
		foreach (InventorySlot slot in slots)
		{
			if (slot.Item == item)
			{
				return slot;
			}
		}

		return null;
	}

	private int GetNumberOfItem(Item item)
	{
		int total = 0;

		foreach (InventorySlot slot in slots)
		{
			if (slot.Item == item)
			{
				total += slot.Quantity;
			}
		}

		return total;
	}
}
