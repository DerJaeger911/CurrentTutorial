using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.AutoLoads.Managers;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Enemies;
using Dcozysandbox.Scripts.Enums;
using Dcozysandbox.Scripts.LookUps;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Game : Node2D
{
	[Export]
	private Gradient dayTimeGradient;
	[Export]
	private Curve musicCurve;
	private CanvasModulate canvasModulate;
	private Timer dayTimer;
	private Timer blobSpawnTimer;
	private CanvasLayer canvasLayer;
	private ColorRect dayTransition;
	private AudioStreamPlayer music;
	private Node2D gameObjects;
	private Node blobSpawnPositions;
	private TileMapLayer waterLayer;
	private TileMapLayer grassLayer;
	private TileMapLayer soilLayer;
	private TileMapLayer soilWaterLayer;
	private TileMapLayer wallsLayer;
	private TileMapLayer floorLayer;
	private Layers layers;
	private bool isRaining;
	private GpuParticles2D rainFloor;
	private GpuParticles2D rain;
	private AudioStreamPlayer rainSound;
	private BuildOverlay buildOverlay;
	private CharacterBody2D player;
	private ResourceUi resourceUi;

	private double daytimePoint = 0;
	private bool fadeOut;

	public override void _Ready()
	{
		this.gameObjects = this.GetNode<Node2D>("Objects");
		this.layers = this.GetNode<Layers>("Layers");
		this.canvasLayer = this.GetNode<CanvasLayer>("CanvasLayer");
		this.dayTransition = this.canvasLayer.GetNode<ColorRect>("DayTransition");
		this.canvasModulate = this.GetNode<CanvasModulate>("CanvasModulate");
		this.dayTimer = this.GetNode<Timer>("Timer/DayTimer");
		this.blobSpawnTimer = this.GetNode<Timer>("Timer/BlobSpawnTimer");
		this.blobSpawnPositions = this.GetNode<Node>("BlobSpawnPositions");
		this.music = this.GetNode<AudioStreamPlayer>("Music");
		this.waterLayer = this.GetNode<TileMapLayer>("Layers/WaterLayer");
		this.grassLayer = this.GetNode<TileMapLayer>("Layers/GrassLayer");
		this.soilLayer = this.GetNode<TileMapLayer>("Layers/SoilLayer");
		this.soilWaterLayer = this.GetNode<TileMapLayer>("Layers/SoilWaterLayer");
		this.floorLayer = this.GetNode<TileMapLayer>("Layers/HouseFloorLayer");
		this.wallsLayer = this.GetNode<TileMapLayer>("Objects/WallLayer");
		this.rain = this.GetNode<GpuParticles2D>("Overlay/RainParticles");
		this.rainFloor = this.GetNode<GpuParticles2D>("Layers/FloorRainParticles");
		this.rainSound = this.GetNode<AudioStreamPlayer>("RainSound");
		this.buildOverlay = this.GetNode<BuildOverlay>("Overlay/BuildOverlay");
		this.player = this.GetNode<CharacterBody2D>("Objects/Player");
		this.resourceUi = this.GetNode<ResourceUi>("CanvasLayer/ResourceUi");
		SignalBus.Instance.ToolInteract += this.OnToolInteract;
		SignalBus.Instance.SeedInteract += this.OnSeedInteract;
		this.blobSpawnTimer.Timeout += this.OnBlobSpawnTimerTimeOut;
		SignalBus.Instance.BuildMode += this.OnPlayerBuildMode;
		SignalBus.Instance.Build += this.OnBuild;
		SignalBus.Instance.DeleteBuild += this.OnDeleteBuild;
		this.isRaining = GD.Randf() > 0.5f;
		this.RainEmit();
		SignalBus.Instance.DoorChanged += this.DoorHandler;
		this.canvasLayer.Visible = true;
	}

	public override void _Process(Double delta)
	{
		this.daytimePoint = 1 - (this.dayTimer.TimeLeft / this.dayTimer.WaitTime);
		this.canvasModulate.Color = this.dayTimeGradient.Sample((float)this.daytimePoint);
		if (!this.fadeOut)
		{
			this.music.VolumeDb = this.musicCurve.Sample((float)this.daytimePoint);
		}

		if (Input.IsActionJustPressed("ui_focus_next"))
		{
			this.DayRestart();
		}
	}

	private void DayRestart()
	{
		this.fadeOut = true;
		Tween tween = this.GetTree().CreateTween();
		tween.Parallel().TweenProperty(this.music, "volume_db", -80.0f, 1.0f);
		tween.Parallel().TweenProperty(this.dayTransition.Material, "shader_parameter/progress", 1, 1);
		tween.TweenInterval(0.5);
		tween.TweenCallback(Callable.From(this.ResetLevel));
		tween.Parallel().TweenProperty(this.music, "volume_db", this.musicCurve.Sample((float)this.daytimePoint), 1.0f);
		tween.Parallel().TweenProperty(this.dayTransition.Material, "shader_parameter/progress", 0.0f, 1.0f);
		tween.TweenCallback(Callable.From(new Action(() => { this.fadeOut = false; })));
	}

	private void ResetLevel()
	{
		this.dayTimer.Start();
		this.music.Play();
		this.isRaining = GD.Randf() > 0.5f;
		this.isRaining = false;
		this.RainEmit();
		if (this.isRaining)
		{
			foreach(var cell in this.soilLayer.GetUsedCells())
			{
				this.soilWaterLayer.SetCell(cell, 0, new Vector2I(GD.RandRange(0, 2), 0));
			}
		}
		foreach(Tree tree in this.GetTree().GetNodesInGroup("Trees"))
		{
			tree.Reset();
		}
		foreach(Plant plant in this.GetTree().GetNodesInGroup("Plants"))
		{
			bool isWatered = this.soilWaterLayer.GetUsedCells().Contains(plant.SoilGridCell);
			plant.Grow(isWatered);
		}
		this.soilWaterLayer.Clear();
	}

	private void OnToolInteract(int tool, Vector2 position)
	{
		Vector2I SoilgridPosition;
		TileData Soildata;
		switch (ToolConstants.All[tool])
		{
			case ToolConstants.Hoe:
				Vector2I grassGridPosition = this.grassLayer.LocalToMap(this.grassLayer.ToLocal(position));
				TileData grassData = this.grassLayer.GetCellTileData(grassGridPosition);
				if (this.grassLayer is not null && (bool)grassData.GetCustomData("farmable") == true)
				{
					this.layers.AddSoil(grassGridPosition);
				}
				if (this.isRaining)
				{
					SoilgridPosition = this.soilLayer.LocalToMap(this.soilLayer.ToLocal(position));
					Soildata = this.soilLayer.GetCellTileData(SoilgridPosition);
					if (Soildata != null)
					{
						this.soilWaterLayer.SetCell(SoilgridPosition, 0, new Vector2I(GD.RandRange(0, 2), 0));
					}
				}
				break;
			case ToolConstants.Axe:
				foreach (Tree tree in this.GetTree().GetNodesInGroup("Trees"))
				{
					if (tree.Position.DistanceTo(position) < 16)
					{
						tree.TakeDamage(1);
					}
				}
				break;
			case ToolConstants.Sword:
				foreach (Enemy enemy in this.GetTree().GetNodesInGroup("Enemies"))
				{
					if (enemy.Position.DistanceTo(position) < 12)
					{
						enemy.TakeDamage(1);
					}
				}
				break;
			case ToolConstants.Fish:
				this.CheckFishing(position);
				break;
			case ToolConstants.Water:
				SoilgridPosition = this.soilLayer.LocalToMap(this.soilLayer.ToLocal(position));
				Soildata = this.soilLayer.GetCellTileData(SoilgridPosition);
				if (Soildata != null) 
				{
					this.soilWaterLayer.SetCell(SoilgridPosition, 0, new Vector2I(GD.RandRange(0, 2), 0));
				}

				break;
		}
	}

	private void OnSeedInteract(int seed, Vector2 position)
	{
		int seedsToUse = 1;
		ResourceEnum currentResource = SeedResourceLookup.ResourceSeedConnection[(SeedEnum)seed];
		if (PlayerResourceManager.Instance.CheckResource(currentResource, seedsToUse))
		{
			Vector2I soilGridPosition = this.soilLayer.LocalToMap(this.soilLayer.ToLocal(position));
			TileData soildata = this.soilLayer.GetCellTileData(soilGridPosition);

			foreach (var floor in this.floorLayer.GetUsedCells())
			{
				if (floor == soilGridPosition)
				{
					return;
				}
			}

			List<Vector2I> existingPlantCells = [];
			foreach (Plant plant in this.GetTree().GetNodesInGroup("Plants"))
			{
				existingPlantCells.Add(plant.SoilGridCell);
			}

			if (soildata != null && !existingPlantCells.Contains(soilGridPosition))
			{
				Plant plant = ScenePreloadManager.Instance.Instantiate<Plant>(PreloadEnum.Plant);
				this.gameObjects.CallDeferred(Node.MethodName.AddChild, plant);
				Vector2 worldPosition = this.soilLayer.MapToLocal(soilGridPosition);
				plant.GlobalPosition = this.soilLayer.ToGlobal(worldPosition - new Vector2(0, 12));
				PlayerResourceManager.Instance.SubtractResource(currentResource, seedsToUse);
				plant.Setup(seed, soilGridPosition);
			}
		}
	}

	private void CheckFishing(Vector2 position)
	{
		if (this.waterLayer == null)
		{
			return;
		}
		Vector2I gridPosition = this.waterLayer.LocalToMap(this.waterLayer.ToLocal(position));
		TileData data = this.waterLayer.GetCellTileData(gridPosition);

		if (data == null)
		{
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.CanFish, false);
		}
		else
		{
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.CanFish, true);
		}
	}

	private void OnBlobSpawnTimerTimeOut()
	{
		Blob blob = ScenePreloadManager.Instance.Instantiate<Blob>(PreloadEnum.Blob);
		this.gameObjects.CallDeferred(Node.MethodName.AddChild, blob);
		var spawnPoints = this.blobSpawnPositions.GetChildren().OfType<Marker2D>().ToList();

		if (spawnPoints.Count > 0)
		{
			int randomIndex = GD.RandRange(0, spawnPoints.Count - 1);
			var randomMarker = spawnPoints[randomIndex];

			blob.GlobalPosition = randomMarker.GlobalPosition;
		}
	}

	private void RainEmit()
	{
		this.rain.Emitting = this.isRaining;
		this.rainFloor.Emitting = this.isRaining;
		if (this.isRaining)
		{
			this.rainSound.Play();
		}
		else
		{
			this.rainSound.Stop();
		}
	}

	private void OnPlayerBuildMode()
	{
		this.dayTimer.Paused = true;
		this.buildOverlay.Reveal(this.player.GlobalPosition);
		this.resourceUi.TweenAnimation(1);
	}

	private void OnBuild(Vector2I position, ObjectEnum buildObject)
	{
		if (buildObject == ObjectEnum.Walls)
		{
			this.wallsLayer.SetCellsTerrainConnect([position], 0, 0);
			this.floorLayer.SetCell(position, 0, Vector2I.Zero);
		}

		if (buildObject == ObjectEnum.Door)
		{
			TileData currentCell = (TileData)this.wallsLayer.GetCellTileData(position);
			if (currentCell != null)
			{
				bool isDoor = (bool)currentCell.GetCustomData("Door");
				if (!isDoor)
				{
					this.wallsLayer.SetCell(position, 0, new Vector2I(0, 4));
					DoorChecker doorChecker = ScenePreloadManager.Instance.Instantiate<DoorChecker>(PreloadEnum.Doorchecker);
					this.gameObjects.CallDeferred(Node.MethodName.AddChild, doorChecker);
					doorChecker.Setup(position);
				}
			}
		}

		if (buildObject is not ObjectEnum.Walls and not ObjectEnum.Door)
		{
			foreach (Node obj in this.GetTree().GetNodesInGroup("Objects"))
			{
				if (obj is BuildObject newBo)
				{
					GD.Print("jo");
					if (newBo.CanDelete(position))
					{
						obj.QueueFree();
					}
				}
			}

			BuildObject buildObjectScene = ScenePreloadManager.Instance.Instantiate<BuildObject>(PreloadEnum.Buildobject);
			buildObjectScene.Setup(buildObject);
			Node2D targetGroup = this.gameObjects;
			if (buildObject == ObjectEnum.Carpet)
			{
				targetGroup = this.GetNode<Node2D>("Layers/CarpetLayer");
			}
			
			targetGroup.CallDeferred(Node.MethodName.AddChild, buildObjectScene);
			buildObjectScene.Position = position * 16 + new Vector2I(8, 8);
		}
	}

	private void OnDeleteBuild(Vector2I position)
	{
		foreach(Node node in this.GetTree().GetNodesInGroup("Objects"))
		{
			if (node is BuildObject buildObject)
			{
				if (buildObject.CanDelete(position))
				{
					node.QueueFree();
					return;
				}
			}
		}

		TileData tileData = (TileData)this.wallsLayer.GetCellTileData(position);

		if (tileData != null && (bool)tileData.GetCustomData("Door"))
		{
			foreach (DoorChecker doorChecker in this.GetTree().GetNodesInGroup("DoorChecker"))
			{
				if (doorChecker.DoorCoordinate == position)
				{
					doorChecker.QueueFree();
					this.floorLayer.SetCell(position, 0, Vector2I.Zero);
					this.wallsLayer.SetCellsTerrainConnect([position], 0, 0);
					return;
				}
			}
		}

		this.floorLayer.EraseCell(position);
		this.wallsLayer.SetCellsTerrainConnect([position], 0, -1);
	}

	private void DoorHandler(Vector2I doorCordinate,bool isOpen)
	{
		GD.Print(doorCordinate);
		GD.Print(isOpen);
		if (isOpen)
		{
			this.wallsLayer.SetCell(doorCordinate, 0, new Vector2I(1, 1));
		}
		else
		{
			this.wallsLayer.SetCell(doorCordinate, 0, new Vector2I(0, 4));
		}
	}

	public override void _ExitTree()
	{
		SignalBus.Instance.ToolInteract -= this.OnToolInteract;
		this.blobSpawnTimer.Timeout -= this.OnBlobSpawnTimerTimeOut;
		SignalBus.Instance.SeedInteract -= this.OnSeedInteract;
		SignalBus.Instance.BuildMode -= this.OnPlayerBuildMode;
		SignalBus.Instance.Build -= this.OnBuild;
		SignalBus.Instance.DeleteBuild -= this.OnDeleteBuild;
	}
}