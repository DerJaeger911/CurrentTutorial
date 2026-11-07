using Godot;
using System;

public partial class InventorySlot : Button
{
	[Export]
	private TextureRect icon;

	[Export]
	private Label quantityText;

	private Player player;

    private Inventory inventory;
    public Inventory Inventory 
    {
        get => this.inventory;
        set 
        {
            this.inventory = value;
            if (this.inventory != null)
            {
                this.player = (Player)this.inventory.GetParent();
            }
        }
    }

	public int Quantity { get; set; }

	public Item Item { get; set; }

    public override void _Ready()
    {
        this.MouseEntered += this.OnMouseEntered;
		this.MouseExited += this.OnMouseExited;
		this.Pressed += this.OnPressed;
		this.GuiInput += this.OnGuiInput;
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

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left)
			{
				
			}
			else if (mouseButton.ButtonIndex == MouseButton.Right)
			{
				this.DropItem();
			}
		}
	}

	private void DropItem()
	{
		if(this.Item == null)
		{
			return;
		}

		RigidBody3D worldItem = (RigidBody3D)this.Item.WorldItemScene.Instantiate();
		this.AddChild(worldItem);
		worldItem.Position = this.player.Position + new Vector3(0, 1.5f, 0) - this.player.Basis.Z;
		this.RemoveItem();
	}
}
