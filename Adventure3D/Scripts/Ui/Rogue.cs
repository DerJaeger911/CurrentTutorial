using Adventuregame.Scripts.GlobalData.Enums;
using Adventuregame.Scripts.GlobalData.ObjectDataClasses;
using Adventuregame.Scripts.Ui;
using Godot;
using System;

public partial class Rogue : Node3D
{
	private BoneAttachment3D headAttatchment;
	private EquipmentSlotRecord slot = new EquipmentSlotRecord("Rig/Skeleton3D/RightHand", "Rig/Skeleton3D/LeftHand", "Rig/Skeleton3D/Head");

	override public void _Ready()
	{
		this.headAttatchment = this.GetNode<BoneAttachment3D>(this.slot.Style);
	}

	public void AddEquipment(ItemDataBase data)
	{
		string slotPath = data.Type switch
		{
			ItemTypeEnum.Weapon => this.slot.Weapon,
			ItemTypeEnum.Shield => this.slot.Shield,
			ItemTypeEnum.Style => this.slot.Style,
			_ => throw new Exception("Unbekannter Item-Typ!")
		};

		var slotNode = this.GetNode<BoneAttachment3D>(slotPath);

		foreach (Node child in slotNode.GetChildren())
		{
			child.QueueFree();
		}

		var itemInstance = data.Scene.Instantiate<Node3D>();
		slotNode.AddChild(itemInstance);
	}

	public void Unequip()
	{
		foreach (Node child in this.headAttatchment.GetChildren())
		{
			child.QueueFree();
		}
	}
}
