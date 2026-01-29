using Godot;
using System;

namespace Adventuregame.Scripts.Items;
public partial class Weapon : Node3D
{
    public const string EquipmentType = "Weapon";
    private string animation;
	private int damage;
	private float radius;
	private Node3D parent;

	public void Setup(string animation, int damage, float radius, Node3D parent)
	{
		this.animation = animation;
		this.damage = damage;
		this.radius = radius;
		this.parent = parent;
	}
}
