using Dcozysandbox.Scripts.Constants.Paths;
using Dcozysandbox.Scripts.Enums;
using Dcozysandbox.Scripts.Objects;
using Godot;
using System;
using System.Collections.Generic;

public partial class Plant : StaticBody2D
{
	private SeedEnum currentSeed;
	private Sprite2D currentPlantSprite;
	private int age;
	private int maxAge = 3;
	private PlantData plantData;

	private readonly Dictionary<SeedEnum, PlantData> Plants = new() 
	{ 
		{SeedEnum.Corn, new (TexturePaths.Corn, 0.5f)}, 
		{SeedEnum.Tomato, new (TexturePaths.Tomato, 0.8f)},
		{SeedEnum.Pumpkin, new (TexturePaths.Pumpkin, 0.9f )},
	};

    public Vector2I SoilGridCell { get; set; }

    public void Setup(int seed, Vector2I gridPosition)
	{
		this.currentPlantSprite = this.GetNode<Sprite2D>("Sprite2D");
		this.SoilGridCell = gridPosition;
		this.currentSeed = (SeedEnum)seed;
		this.plantData = this.Plants[this.currentSeed];

		this.currentPlantSprite.Texture = GD.Load<Texture2D>(this.plantData.Path);
	}

	public void Grow(bool watered)
	{
		if (watered && this.age <= this.maxAge)
		{
			this.age += (int)Mathf.Floor(this.plantData.GrowthSpeed);
			this.currentPlantSprite.Frame = this.age;
		}
	}
}
