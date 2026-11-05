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

    public override void _Ready()
    {
        this.MouseEntered += this.OnMouseEntered;
		this.MouseExited += this.OnMouseExited;
		this.Pressed += this.OnPressed;
    }

	public void SetItem(Item item)
	{
		this.Item = item;
		this.Quantity = 1;

		if (item == null)
		{
            this.icon.Visible = false;
		}
		else
		{
            this.icon.Visible = true;
            this.icon.Texture = item.Icon;
		}

		this.UpdateQuantityText();
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

		if(this.Quantity == 0)
		{
			this.SetItem(null);
		}
	}

	private void UpdateQuantityText()
	{
		if (this.Quantity <= 1)
		{
            this.quantityText.Text = "";
		}
		else
		{
			this.quantityText.Text = this.Quantity.ToString();
		}
	}

	private void OnMouseEntered()
	{
		if(this.Item == null)
		{
            this.Inventory.InfoText.Text = "";
		}
		else
		{
            this.Inventory.InfoText.Text = this.Item.DisplayName;
		}
	}

	private void OnMouseExited()
	{
        this.Inventory.InfoText.Text = "";
	}

	private void OnPressed()
	{
		if (this.Item == null)
		{
			return;
		}

		bool removeAfterUse = this.Item.OnUse((Player)this.Inventory.GetParent());

		if (removeAfterUse)
		{
			this.RemoveItem();
		}
	}

	private void DropItem()
	{
		
	}
}
