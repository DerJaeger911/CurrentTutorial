using Adventuregame.Scripts.Characters.Player;
using Adventuregame.Scripts.GlobalData.Enums;
using Adventuregame.Scripts.GlobalData.ObjectDataClasses;
using Adventuregame.Scripts.Ui;
using Godot;
using System;

public partial class Item : Button
{
	private TextureRect textureRect;
	private Panel equipmentPanel;
	private Rogue playerPreview;
	private Player player;
	private ItemDataBase equipmentData;
	private EquipmentSlotRecord slot = new EquipmentSlotRecord("RightHand", "LeftHand", "Head");

	override public void _Ready()
	{
		this.playerPreview = (Rogue)this.GetTree().GetFirstNodeInGroup("PlayerPreview");
		this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");
		this.SizeFlagsHorizontal = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
		this.EnsureInitialisation();
		this.Resized += this.OnResize;
		this.equipmentPanel = this.GetNode<Panel>("EquipmentPanel");
		this.Pressed += this.OnButtonPressed;
	}

	public void Setup(ItemDataBase data)
	{
		this.EnsureInitialisation();
		this.equipmentData = data;
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

	private void OnButtonPressed()
	{
		string targetSlot = this.equipmentData.Type switch
		{
			ItemTypeEnum.Weapon => this.slot.Weapon,
			ItemTypeEnum.Shield => this.slot.Shield,
			ItemTypeEnum.Style => this.slot.Style,
			_ => throw new Exception("Invalid item type")
		};
		this.player.Equip(this.equipmentData, (BoneAttachment3D)this.player.GetNode("PlayerSkin/Rogue/Rig/Skeleton3D/" + targetSlot));
		this.playerPreview.AddEquipment(this.equipmentData);
		foreach(Item item in this.GetParent().GetChildren())
		{
			item.Highlight(false);
		}
		this.Highlight(true);
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

	override public void _ExitTree()
	{
		this.Resized -= this.OnResize;
		this.Pressed -= this.OnButtonPressed;
	}
}
