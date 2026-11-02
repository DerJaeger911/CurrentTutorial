using Godot;
using System;

public partial class InventorySlot : Button
{
	[Export]
	private TextureRect icon;

	[Export]
	private Label quantityText;

	public Inventory Inventory { get; set; }

	public int Quantity { get; set; }

	public Item Item { get; set; }

	public void SetItem(Item item)
	{
		this.Item = item;
		this.Quantity = 1;

		if (item == null)
		{
			icon.Visible = false;
		}
		else
		{
			icon.Visible = true;
			icon.Texture = item.Icon;
		}
	}

	public void AddItem()
	{
		this.Quantity += 1;
		this.UpdateQuantityText();
	}

	public void RemoveItem()
	{
		this.Quantity -= 1;
		this.UpdateQuantityText();
	}

	private void UpdateQuantityText()
	{
		if (this.Quantity <= 1)
		{
			quantityText.Text = "";
		}
		else
		{
			this.quantityText.Text = this.Quantity.ToString();
		}
	}
}
