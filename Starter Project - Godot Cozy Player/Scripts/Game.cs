using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.AutoLoads.Managers;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Enemies;
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
	private Layers layers;
	private bool isRaining;
	private GpuParticles2D rainFloor;
	private GpuParticles2D rain;
	private AudioStreamPlayer rainSound;



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
		this.rain = this.GetNode<GpuParticles2D>("Overlay/RainParticles");
		this.rainFloor = this.GetNode<GpuParticles2D>("Layers/FloorRainParticles");
		this.rainSound = this.GetNode<AudioStreamPlayer>("RainSound");
		SignalBus.Instance.ToolInteract += this.OnToolInteract;
		SignalBus.Instance.SeedInteract += this.OnSeedInteract;
		this.blobSpawnTimer.Timeout += this.OnBlobSpawnTimerTimeOut;
		this.isRaining = GD.Randf() > 0.5f;
		this.RainEmit();
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
		Vector2I soilGridPosition = this.soilLayer.LocalToMap(this.soilLayer.ToLocal(position));
		TileData soildata = this.soilLayer.GetCellTileData(soilGridPosition);

		List<Vector2I> existingPlantCells = [];
		foreach(Plant plant in this.GetTree().GetNodesInGroup("Plants"))
		{
			existingPlantCells.Add(plant.SoilGridCell);
		}

		if (soildata != null && !existingPlantCells.Contains(soilGridPosition))
		{
			Plant plant = ScenePreloadManager.Instance.Instantiate<Plant>(PreloadEnum.Plant);
			this.gameObjects.CallDeferred(Node.MethodName.AddChild, plant);
			Vector2 worldPosition = this.soilLayer.MapToLocal(soilGridPosition);
			plant.GlobalPosition = this.soilLayer.ToGlobal(worldPosition - new Vector2 (0, 12));
			plant.Setup(seed, soilGridPosition);
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


	public override void _ExitTree()
	{
		SignalBus.Instance.ToolInteract -= this.OnToolInteract;
		this.blobSpawnTimer.Timeout -= this.OnBlobSpawnTimerTimeOut;
		SignalBus.Instance.SeedInteract -= this.OnSeedInteract;

	}
}