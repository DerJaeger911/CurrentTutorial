using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

namespace Adventuregame.Scripts.Items;
public partial class Weapon : Node3D
{
    public static ItemTypeEnum EquipmentType = ItemTypeEnum.Weapon;
    private string animation;
	private AudioStreamPlayer3D attackSound;
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

	override public void _Ready()
	{
		this.attackSound = this.GetNode<AudioStreamPlayer3D>("AttackSound");
	}

	public GodotObject GetCollider()
	{
		var ray = this.GetNode<RayCast3D>("RayCast3D");

		ray.ForceRaycastUpdate();

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

	public void SetSound(AudioStream audio)
	{
		this.attackSound.Stream = audio;
	}

	public void PlaySound()
	{
		this.attackSound.Play();
	}
}
