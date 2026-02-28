using Godot;
using Godot.Collections;

namespace twentyfourtyeight.src.main.Mangager;

internal partial class AssetManager : Node
{
	public static AssetManager Instance { get; private set; }
	private readonly Dictionary<TerrainEnum, Texture2D> terrainTextures = new();

	public override void _Ready()
	{
		Instance = this;
		this.LoadAllResources();
	}

	private void LoadAllResources()
	{
		this.terrainTextures[TerrainEnum.Plains] = (Texture2D)GD.Load("res://Assets-4XHexMap/plains.jpg");
		this.terrainTextures[TerrainEnum.Mountain] = (Texture2D)GD.Load("res://Assets-4XHexMap/mountain.jpg");
		this.terrainTextures[TerrainEnum.Forest] = (Texture2D)GD.Load("res://Assets-4XHexMap/forest.jpg");
		this.terrainTextures[TerrainEnum.Beach] = (Texture2D)GD.Load("res://Assets-4XHexMap/beach.jpg");
		this.terrainTextures[TerrainEnum.Ice] = (Texture2D)GD.Load("res://Assets-4XHexMap/ice.jpg");
		this.terrainTextures[TerrainEnum.Desert] = (Texture2D)GD.Load("res://Assets-4XHexMap/desert.jpg");
		this.terrainTextures[TerrainEnum.Water] = (Texture2D)GD.Load("res://Assets-4XHexMap/ocean.jpg");
		this.terrainTextures[TerrainEnum.ShallowWater] = (Texture2D)GD.Load("res://Assets-4XHexMap/shallow.jpg");

	}

	public Texture2D GetTerrainTexture(TerrainEnum type)
	{
		if (this.terrainTextures.TryGetValue(type, out var tex))
		{
			return tex;
		}
		GD.PushError($"AssetManager: Textur für {type} nicht gefunden!");
		return null;
	}

	public void Unload()
	{
		this.terrainTextures.Clear();
		AssetManager.Instance = null;
		this.QueueFree();
		GD.Print("AssetManager: Ressourcen entladen.");
	}
}
