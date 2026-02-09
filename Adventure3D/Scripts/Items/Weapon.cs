using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

namespace Adventuregame.Scripts.Items;
public partial class Weapon : Node3D
{
    public static ItemTypeEnum EquipmentType = ItemTypeEnum.Weapon;
    private string animation;
	public int Damage { get; set; }
	public float Radius {  get; set; }
	private Node3D parent;

	public void Setup(string animation, int damage, float radius, Node3D parent)
	{
		this.animation = animation;
		this.Damage = damage;
		this.Radius = radius;
		this.parent = parent;
	}

	public GodotObject GetCollider()
	{
		var ray = this.GetNode<RayCast3D>("RayCast3D");

		// 1. Force the physics state to sync
		ray.ForceRaycastUpdate();

		// 2. Check the boolean first
		if (ray.IsColliding())
		{
			var obj = ray.GetCollider();
			if (obj != null)
			{
				return obj;
			}
		}
		return null;
	}
}
