using Godot;
using System;
using System.Collections.Generic;
using twentyfourtyeight.src.main;
using twentyfourtyeight.src.main.SignalHubs;

public partial class City : Node2D
{
	private Label label;
	private Sprite2D sprite;

	private int population = 1;

	public static Dictionary<Hex, City> InvalidTiles = new Dictionary<Hex, City>();

	public HexTileMap Map {  get; set; }
	public Vector2I CenterCoordinates { get; set; }
	public List<Hex> Territory {  get; set; } = new List<Hex>();
	public List<Hex> BorderTilePool { get; set; } = new List<Hex>();
	public List<Unit> UnitBuildQueue { get; set; } = new List<Unit>();
	public Unit CurrentUnitBeingBuilt { get; set; }
	public int UnitBuildTracker { get; set; }
	public string CityName { get; set; }
	public Civilisation Civ {  get; set; }
	public Int32 Population { get => this.population; set => this.population = value; }
	public int TotalFood { get; set; }
	public int TotalProduction { get; set; }
	public int PopulationGrowthThreshold { get; set; }
	public int PopulationGrowthTracker { get; set; }

	private const int populationThreshholdIncrease = 15;

	public override void _Ready()
	{
		this.label = this.GetNode<Label>("Label");
		this.sprite = this.GetNode<Sprite2D>("Sprite2D");
		this.label.Text = this.CityName;
	}

	public void ProcessTurn()
	{
		this.CleanUpBoarderPool();
		this.PopulationGrowthTracker += this.TotalFood;
		if(this.PopulationGrowthTracker > this.PopulationGrowthThreshold)
		{
			this.Population++;
			this.PopulationGrowthTracker = 0;
			this.PopulationGrowthThreshold += populationThreshholdIncrease;
			this.AddRandomNewTile();
			this.Map.UpdateCivTerritoryMap(this.Civ);
		}
		this.ProcessUnitBuildQueue();
	}

	public void CleanUpBoarderPool()
	{
		List<Hex> toRemove = new List<Hex>();
		foreach (Hex hex in this.BorderTilePool)
		{
			if (InvalidTiles.ContainsKey(hex) && InvalidTiles[hex] != this)
			{
				toRemove.Add(hex);
			}
		}

		foreach(Hex hex in toRemove)
		{
			this.BorderTilePool.Remove(hex);
		}
	}

	public void AddValidNeighborsToBorderPool(Hex hex)
	{
		List<Hex> neighbors = this.Map.GetSurroundingHexes(hex.Coordinates);

		foreach(Hex neighborHex in neighbors)
		{
			if (this.IsValidNeighborTile(neighborHex))
			{
				this.BorderTilePool.Add(neighborHex);
				InvalidTiles[neighborHex] = this;
			}
		}
	}

	public void SpawnUnit(Unit unit)
	{
		Unit unitToSpawn = (Unit)Unit.UnitSceneResources[unit.GetType()].Instantiate();
		this.Map.AddChild(unitToSpawn);
		unitToSpawn.Position = this.Map.MapToLocal(this.CenterCoordinates);
		unitToSpawn.SetCiv(this.Civ);
		unitToSpawn.Coords = this.CenterCoordinates;
	}

	public bool IsValidNeighborTile(Hex neighborHex)
	{
		if(neighborHex.TerrainType == TerrainEnum.Water ||
			neighborHex.TerrainType == TerrainEnum.Ice||
			neighborHex.TerrainType == TerrainEnum.ShallowWater||
			neighborHex.TerrainType == TerrainEnum.Mountain)
		{
			return false; 
		}

		if (neighborHex.OwnerCity != null && neighborHex.OwnerCity.Civ != null)
		{
			return false;
		}

		if(InvalidTiles.ContainsKey(neighborHex) && InvalidTiles[neighborHex] != this)
		{
			return false;
		}

		return true;
	}

	public void CalculateTerritoryResourceTotals()
	{
		this.TotalFood = 0;
		this.TotalProduction = 0;

		foreach(Hex hex in this.Territory)
		{
			this.TotalFood += hex.Food;
			this.TotalProduction += hex.Production;
		}
	}

	public void AddUnitToBuildQueue(Unit unit)
	{
		this.UnitBuildQueue.Add(unit);
	}

	public void AddRandomNewTile()
	{
		if (this.BorderTilePool.Count > 0)
		{
			Random rnd = new Random();
			int index = rnd.Next(this.BorderTilePool.Count);
			this.AddTerritory(new List<Hex> { this.BorderTilePool[index] });
			this.BorderTilePool.RemoveAt(index);
		}
	}

	public void AddTerritory(List<Hex> territoryToAdd)
	{
		foreach (Hex hex in territoryToAdd) 
		{
			hex.OwnerCity = this;
			this.AddValidNeighborsToBorderPool(hex);
		}

		this.Territory.AddRange(territoryToAdd);
		this.CalculateTerritoryResourceTotals();
	} 

	public void ProcessUnitBuildQueue()
	{
		if(this.UnitBuildQueue.Count > 0)
		{
			if(this.CurrentUnitBeingBuilt == null)
			{
				this.CurrentUnitBeingBuilt = this.UnitBuildQueue[0];
			}

			this.UnitBuildTracker += this.TotalProduction;
			
			if (this.UnitBuildTracker >= this.CurrentUnitBeingBuilt.ProductionCost)
			{
				this.SpawnUnit(this.CurrentUnitBeingBuilt);
				this.UnitBuildQueue.RemoveAt(0);
				this.CurrentUnitBeingBuilt = null;
				this.UnitBuildTracker = 0;
			}
		}
	}

	public void SetCityName(string cityName)
	{
		this.CityName = cityName;
		this.label.Text = cityName;
	}

	public void SetIconColor(Color color)
	{
		this.sprite.Modulate = color;
	}
}
