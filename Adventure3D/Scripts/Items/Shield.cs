using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

public partial class Shield : Node3D
{
	public static ItemTypeEnum EquipmentType = ItemTypeEnum.Shield;
	public float Defense { get; set; }

	private ShaderMaterial flashMaterial;
	private AudioStreamPlayer3D blockSound;

	public void Setup( float defense)
	{
		this.Defense = defense;
	}

	override public void _Ready()
	{
		MeshInstance3D mesh = (MeshInstance3D)this.GetChild(0).GetChild(0);
		this.flashMaterial = (ShaderMaterial)mesh.MaterialOverlay;
		this.blockSound = this.GetNode<AudioStreamPlayer3D>("BlockSound");
	}

	public void Flash() 
	{
		Tween tween = this.CreateTween();
		tween.TweenMethod(Callable.From<float>(this.InternalFlash), 0.0f, 1.0f, 0.1f);
		tween.TweenMethod(Callable.From<float>(this.InternalFlash), 1.0f, 0.0f, 0.2f);
	}

	private void InternalFlash(float value)
	{
		this.flashMaterial.SetShaderParameter("alpha", value);
	}

	public void PlaySound()
	{
		this.blockSound.Play();
	}
}
