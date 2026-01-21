using Dcozysandbox.Scripts.AutoLoads.Managers;
using Dcozysandbox.Scripts.Constants.Paths;
using Dcozysandbox.Scripts.Entities.Player;
using Dcozysandbox.Scripts.Enums;
using Dcozysandbox.Scripts.LookUps;
using Dcozysandbox.Scripts.Objects;
using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;
using System;
using System.Collections.Generic;

public partial class Plant : StaticBody2D
{
	private SeedEnum currentSeed;
	private Sprite2D currentPlantSprite;
	private float age;
	private int maxAge = 3;
	private PlantData plantData;
	private Area2D collectArea;

	private readonly Dictionary<SeedEnum, PlantData> Plants = new() 
	{ 
		{SeedEnum.Corn, new (TexturePaths.Corn, 0.5f)}, 
		{SeedEnum.Tomato, new (TexturePaths.Tomato, 0.8f)},
		{SeedEnum.Pumpkin, new (TexturePaths.Pumpkin, 0.9f )},
	};

    public Vector2I SoilGridCell { get; set; }

    public void Setup(int seed, Vector2I gridPosition)
	{
		PlayerResourceManager.Instance.PrintAll();
		this.currentPlantSprite = this.GetNode<Sprite2D>("Sprite2D");
		this.SoilGridCell = gridPosition;
		this.currentSeed = (SeedEnum)seed;
		this.plantData = this.Plants[this.currentSeed];
		this.collectArea = this.GetNode<Area2D>("CollectArea");

		this.collectArea.CollisionMask = LayerMask.PlantMask;

		this.currentPlantSprite.Texture = GD.Load<Texture2D>(this.plantData.Path);
		this.collectArea.BodyEntered += this.OnBodyEnteredCollectionArea;
	}

	public void Grow(bool watered)
	{
		if (watered && this.age <= this.maxAge)
		{
			this.age += this.plantData.GrowthSpeed;
			this.currentPlantSprite.Frame = (int)Mathf.Floor(this.age);

			GD.Print("Age ", this.age, "  Max Age ", this.maxAge);
		}
	}

	private void OnBodyEnteredCollectionArea(Node body)
	{
		if (body.IsInGroup("Player") && this.age >= this.maxAge)
		{
			PlayerResourceManager.Instance.AddResource(SeedResourceLookup.ResourceSeedConnection[this.currentSeed], 3);
			PlayerResourceManager.Instance.PrintAll();
			this.QueueFree();
		}
	}

	public override void _ExitTree()
    {
		this.collectArea.BodyEntered -= this.OnBodyEnteredCollectionArea;
	}
}
