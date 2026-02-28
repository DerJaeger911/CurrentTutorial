using Godot;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using twentyfourtyeight.src.main;
using twentyfourtyeight.src.main.SeedGeneration;
using twentyfourtyeight.src.main.SignalHubs;
using ResourceRule = (int foodMin, int foodMax, int prodMin, int prodMax);

public partial class HexTileMap : Node2D
{
	[Export]
	private int width = 100;
	[Export]
	private int height = 100;
	[Export]
	private int seed = 100000;
	[Export]
	private int terrianSeed = 100000;
	[Export]
	private int resourceSeed = 100000;
	[Export]
	private int maxIce = 5;
	[Export]
	private bool useRandomizeTerrainSeed;
	[Export]
	private bool useRandomizeResourceSeed;
	[Export]
	private bool useRandomizedIceSeed;
	[Export]
	private bool useTerrainSeedOnly;

	private PackedScene cityScene;

	private UiManager uiManager;

	private SeedGenerator seedGenerator;

	private TileMapLayer baseLayer, borderLayer, overlayLayer, civColorLayer;

	private Vector2I currentSelectedCell = new Vector2I(-1, -1);

	private List<Vector2I> plainsTiles = new List<Vector2I>();

	private Dictionary<Vector2I, Hex> mapData = new Dictionary<Vector2I, Hex>();
	private readonly Dictionary<TerrainEnum, Vector2I> terrainTextures = new Dictionary<TerrainEnum, Vector2I>
	{
		{TerrainEnum.Plains, new Vector2I(0,0)},
		{TerrainEnum.Water, new Vector2I(1,0)},
		{TerrainEnum.Desert, new Vector2I(0,1)},
		{TerrainEnum.Mountain, new Vector2I(1,1)},
		{TerrainEnum.Beach, new Vector2I(0,2)},
		{TerrainEnum.ShallowWater, new Vector2I(1,2)},
		{TerrainEnum.Ice, new Vector2I(0,3)},
		{TerrainEnum.Forest, new Vector2I(1,3)},
		{TerrainEnum.CivColorBase, new Vector2I(0,3)}
	};

	private readonly Dictionary<TerrainEnum, ResourceRule> resourceRules = new()
	{
		{TerrainEnum.Plains, (2,6,0,3) },
		{TerrainEnum.Forest, (1,4,2,6) },
		{TerrainEnum.Desert, (0,2,0,2) },
		{TerrainEnum.Beach, (0,4,0,2 ) }
	};

	public Dictionary<Vector2I, City> cities { get; set; } = new Dictionary<Vector2I, City>();
	public List<Civilisation> civs { get; set; } = new List<Civilisation>();
	public Int32 Width { get => this.width; set => this.width = value; }
	public Int32 Height { get => this.height; set => this.height = value; }

	override public void _Ready()
	{
		this.cityScene = GD.Load<PackedScene>("res://src/main/city.tscn");

		this.baseLayer = this.GetNode<TileMapLayer>("BaseLayer");
		this.borderLayer = this.GetNode<TileMapLayer>("HexBordersLayer");
		this.overlayLayer = this.GetNode<TileMapLayer>("SelectionOverlayLayer");
		this.civColorLayer = this.GetNode<TileMapLayer>("CivColorLayer");

		this.uiManager = this.GetNode<UiManager>("/root/Game/CanvasLayer/UiManager");

		SeedSettings.Instance.SetSeedSettings(this.resourceSeed, this.terrianSeed, this.maxIce, this.useRandomizeResourceSeed, this.useRandomizeTerrainSeed, this.useRandomizedIceSeed, this.useTerrainSeedOnly);

		this.seedGenerator = new();

		this.GenerateTerrain();
		this.GenerateResources();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse)
		{
			Vector2I mapCoords = this.baseLayer.LocalToMap(this.ToLocal(this.GetGlobalMousePosition()));

			if (mapCoords.X >= 0 && mapCoords.X < this.Width && mapCoords.Y >= 0 && mapCoords.Y < this.Height)
			{
				if (mouse.ButtonMask == MouseButtonMask.Left)
				{
					Hex hex = this.mapData[mapCoords];
					GD.Print(this.mapData[mapCoords]);

					HexSignals.EmitSendHexData(hex);

					if (mapCoords != this.currentSelectedCell)
					{
						this.overlayLayer.SetCell(this.currentSelectedCell, -1);
					}

					this.overlayLayer.SetCell(mapCoords, 0, new Vector2I(0, 1));

					this.currentSelectedCell = mapCoords;
				}
			}
			else
			{
				this.overlayLayer.SetCell(mapCoords, -1);
				HexSignals.EmitClickOffMap();
			}
		}
	}

	public void GenerateResources()
	{
		for (int x = 0; x < this.Width; x++)
		{
			for (int y = 0; y < this.Height; y++)
			{
				Hex hex = this.mapData[new Vector2I(x, y)];
				if (this.resourceRules.TryGetValue(hex.TerrainType, out var rule))
				{
					if (rule.foodMax > 0)
					{
						hex.Food = this.seedGenerator.GetResourceSeed(hex.Coordinates.X, hex.Coordinates.Y, rule.foodMin, rule.foodMax);
					}

					if (rule.prodMax > 0)
					{
						hex.Production = this.seedGenerator.GetResourceSeed(hex.Coordinates.X, hex.Coordinates.Y, rule.prodMin, rule.prodMax);
					}
				}
			}
		}
	}

	public List<Vector2I> GenerateCivStartingLocations(int numLocations)
	{
		List<Vector2I> locations = new List<Vector2I>();

		Random rnd = new Random();
		for (int i = 0; i < numLocations; i++)
		{
			Vector2I coord = new Vector2I();
			bool valid = false;
			int counter = 0;

			while (!valid && counter < 10000)
			{
				coord = this.plainsTiles[rnd.Next(this.plainsTiles.Count)];
				valid = this.IsValidLocation(coord, locations);
				counter++;
			}
			this.plainsTiles.Remove(coord);
			foreach(Hex h in this.GetSurroundingHexes(coord))
			{
				foreach(Hex j in this.GetSurroundingHexes(h.Coordinates))
				{
					foreach(Hex k in this.GetSurroundingHexes(j.Coordinates))
					{
						this.plainsTiles.Remove(h.Coordinates);
						this.plainsTiles.Remove(j.Coordinates);
						this.plainsTiles.Remove(k.Coordinates);
					}
				}
			}
			locations.Add(coord);
		}

		return locations;
	}

	private bool IsValidLocation(Vector2I coord, List<Vector2I> locations)
	{
		if (coord.X < 3 || coord.X >this.Width - 3 ||
			coord.Y < 3|| coord.Y > this.Height - 3)
		{
			return false;
		}

		foreach (Vector2I location in locations)
		{
			if(Math.Abs(coord.X - location.X) < 20 || Math.Abs(coord.Y - location.Y) < 20)
			{
				return false;
			}
		}
		return true;
	}

	public void CreateCity(Civilisation civ, Vector2I coords, string name)
	{
		City city = (City)this.cityScene.Instantiate();
		city.Map = this;
		civ.Cities.Add(city);
		city.Civ = civ;

		this.AddChild(city);

		city.SetIconColor(civ.TerritoryColor);
		city.SetCityName(name);
		city.CenterCoordinates = coords;
		city.Position = this.baseLayer.MapToLocal(coords);
		this.mapData[coords].IsCityCenter = true;
		city.AddTerritory(new List<Hex> { this.mapData[coords] });
		this.plainsTiles.Remove(coords);
		List<Hex> surrounding = this.GetSurroundingHexes(coords);

		foreach (Hex hex in surrounding)
		{
			if (hex.OwnerCity == null)
			{
				city.AddTerritory(new List<Hex> { hex });
				this.plainsTiles.Remove(hex.Coordinates);
			}
		}

		this.UpdateCivTerritoryMap(civ);

		this.cities[coords] = city;
	}

	public void UpdateCivTerritoryMap(Civilisation civ)
	{
		foreach (City city in civ.Cities)
		{
			foreach (Hex hex in city.Territory)
			{
				this.civColorLayer.SetCell(hex.Coordinates, 0, this.terrainTextures[TerrainEnum.CivColorBase], civ.TerritoryColorAltTileId);
			}
		}
	}

	public List<Hex> GetSurroundingHexes(Vector2I coords)
	{
		List<Hex> result = new List<Hex>();

		foreach (Vector2I coord in this.baseLayer.GetSurroundingCells(coords))
		{
			if (this.IsInBounds(coord))
			{
				result.Add(this.mapData[coord]);
			}
		}

		return result;
	}

	public bool IsInBounds(Vector2I coords) =>
	coords.X >= 0 && coords.X < this.width && coords.Y >= 0 && coords.Y < this.height;

	public void GenerateTerrain()
	{
		this.plainsTiles.Clear();

		float[,] noiseMap = new float[this.Width, this.Height];
		float[,] desertMap = new float[this.Width, this.Height];
		float[,] forestMap = new float[this.Width, this.Height];
		float[,] mountainMap = new float[this.Width, this.Height];

		int finalSeed = this.seedGenerator.GetTerrainSeed();



		FastNoiseLite noise = this.CreateNoise(finalSeed, 0.008f, 2.25f, 4, FastNoiseLite.NoiseTypeEnum.Perlin);
		float noiseMax = this.SetNoiseMax(noiseMap, noise);
		FastNoiseLite forestnoise = this.CreateNoise(finalSeed, 0.04f, 2, 1, FastNoiseLite.NoiseTypeEnum.Cellular);
		float forestNoiseMax = this.SetNoiseMax(forestMap, forestnoise);
		FastNoiseLite desertNoise = this.CreateNoise(finalSeed, 0.015f, 2, 1, FastNoiseLite.NoiseTypeEnum.SimplexSmooth);
		float desertNoiseMax = this.SetNoiseMax(desertMap, desertNoise);
		FastNoiseLite mountainNoise = this.CreateNoise(finalSeed, 0.02f, 2, 1, FastNoiseLite.NoiseTypeEnum.Simplex, FastNoiseLite.FractalTypeEnum.Ridged);
		float mountainNoiseMax = this.SetNoiseMax(mountainMap, mountainNoise);


		List<(float Min, float Max, TerrainEnum Type)> terrainGenValues = new List<(float min, float max, TerrainEnum Type)>()
		{
			(0, noiseMax / 10 * 2.5f, TerrainEnum.Water),
			(noiseMax / 10 * 2.5f, noiseMax / 10 * 4, TerrainEnum.ShallowWater),
			(noiseMax / 10 * 4, noiseMax / 10 * 4.5f, TerrainEnum.Beach),
			(noiseMax / 10 * 4.5f, noiseMax + 0.05f, TerrainEnum.Plains)
		};

		Vector2 forestGenValues = new Vector2(forestNoiseMax / 10 * 7, forestNoiseMax + 0.05f);
		Vector2 desertGenValues = new Vector2(desertNoiseMax / 10 * 6, desertNoiseMax + 0.05f);
		Vector2 mountainGenValues = new Vector2(mountainNoiseMax / 10 * 6, mountainNoiseMax + 0.05f);

		var biomeChecks = new (float[,] Map, Vector2 Range, TerrainEnum Type)[]
		{
			(mountainMap, mountainGenValues, TerrainEnum.Mountain),
			(forestMap, forestGenValues, TerrainEnum.Forest),
			(desertMap, desertGenValues, TerrainEnum.Desert),
		};

		for (int x = 0; x < this.Width; x++)
		{
			for (int y = 0; y < this.Height; y++)
			{
				Hex hex = new Hex(new Vector2I(x, y));
				float noiseValue = noiseMap[x, y];

				hex.TerrainType = terrainGenValues.First(range => noiseValue >= range.Min && noiseValue < range.Max).Type;
				this.mapData[new Vector2I(x, y)] = hex;



				if (hex.TerrainType == TerrainEnum.Plains)
				{
					foreach (var check in biomeChecks)
					{
						float val = check.Map[x, y];
						if (val >= check.Range.X && val <= check.Range.Y)
						{
							hex.TerrainType = check.Type;
							break;
						}
					}
				}

				if (hex.TerrainType == TerrainEnum.Plains)
				{
					this.plainsTiles.Add(new Vector2I(x, y));
				}

				this.baseLayer.SetCell(new Vector2I(x, y), 0, this.terrainTextures[hex.TerrainType]);
				this.borderLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
			}
		}

		for (int x = 0; x < this.Width; x++)
		{
			for (int y = 0; y < this.seedGenerator.GetIceSeed(x) + 1; y++)
			{
				Hex hex = this.mapData[new Vector2I(x, y)];
				hex.TerrainType = TerrainEnum.Ice;
				this.baseLayer.SetCell(new Vector2I(x, y), 0, this.terrainTextures[hex.TerrainType]);
			}

			for (int y = this.Height - 1; y > this.Height - 1 - this.seedGenerator.GetIceSeed(x + 5000) - 1; y--)
			{
				Hex hex = this.mapData[new Vector2I(x, y)];
				hex.TerrainType = TerrainEnum.Ice;
				this.baseLayer.SetCell(new Vector2I(x, y), 0, this.terrainTextures[hex.TerrainType]);
			}
		}
	}

	public Vector2 MapToLocal(Vector2I coords)
	{
		return this.baseLayer.MapToLocal(coords);
	}

	private FastNoiseLite CreateNoise(int seed, float frequency, float fractalLacunarity, int fractalOctaves, FastNoiseLite.NoiseTypeEnum noiseType, FastNoiseLite.FractalTypeEnum fractalType = FastNoiseLite.FractalTypeEnum.Fbm)
	{
		FastNoiseLite noise = new FastNoiseLite();
		noise.NoiseType = noiseType;
		noise.SetSeed(seed);
		noise.Frequency = frequency;
		noise.FractalType = fractalType;
		noise.FractalOctaves = fractalOctaves;
		noise.FractalLacunarity = fractalLacunarity;

		return noise;
	}

	private float SetNoiseMax(float[,] noiseMap, FastNoiseLite noise)
	{
		float noiseMax = 0;
		for (int x = 0; x < this.Width; x++)
		{
			for (int y = 0; y < this.Height; y++)
			{
				noiseMap[x, y] = Math.Abs(noise.GetNoise2D(x, y));

				if (noiseMap[x, y] > noiseMax)
				{
					noiseMax = noiseMap[x, y];
				}
			}
		}
		return noiseMax;
	}
}
