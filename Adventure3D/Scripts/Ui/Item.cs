using Adventuregame.Scripts.GlobalData.ObjectDataClasses;
using Godot;
using System;

public partial class Item : Button
{
	private TextureRect textureRect;
	private Panel equipmentPanel;

	override public void _Ready()
	{
		this.SizeFlagsHorizontal = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
		this.EnsureInitialisation();
		this.Resized += this.OnResize;
		this.equipmentPanel = this.GetNode<Panel>("EquipmentPanel");
	}

	public void Setup(ItemDataBase data)
	{
		this.EnsureInitialisation();
		this.textureRect.Texture = data.Thumbnail;
	}

	private void EnsureInitialisation()
	{
		if (this.textureRect == null)
		{
			this.textureRect = this.GetNode<TextureRect>("TextureRect");
		}
	}

	private void OnResize()
	{
		this.CustomMinimumSize = new Vector2(this.CustomMinimumSize.X, this.GetRect().Size.X);
	}

	override public void _ExitTree()
	{
		this.Resized -= this.OnResize;
	}

	public void Highlight(bool value)
	{
		if (value)
		{
			this.equipmentPanel.Show();
		}
		else
		{
			this.equipmentPanel.Hide();
		}
	}
}
