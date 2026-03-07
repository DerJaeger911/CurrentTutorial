using Godot;
using System.Security.AccessControl;
using twentyfourtyeight.src.main;
using twentyfourtyeight.src.main.SignalHubs;

public partial class UiManager : Node2D
{
	private PackedScene terrainUiScene;
	private PackedScene cityUiScene;

	private TerrainTileUi terrainTileUi;

	private CityUi cityTileUi;

	public override void _Ready()
	{
		this.terrainUiScene = ResourceLoader.Load<PackedScene>("res://src/main/Ui/terrain_tile_ui.tscn");
		this.cityUiScene = ResourceLoader.Load<PackedScene>("res://src/main/Ui/CityUi.tscn");

		HexSignals.OnSendHexData += this.SetTerrainUi;
		HexSignals.OnSendCityData += this.SetCityUi;
		HexSignals.OnClickOffMap += this.HideAllPopups;
	}

	public void HideAllPopups()
	{
		if (this.terrainTileUi is not null)
		{
			this.terrainTileUi.QueueFree();
			this.terrainTileUi = null;
		}

		if (this.cityTileUi is not null)
		{
			this.cityTileUi.QueueFree();
			this.cityTileUi = null;
		}
	}

	private void SetCityUi(City city)
	{
		this.HideAllPopups();

		this.cityTileUi = (CityUi)this.cityUiScene.Instantiate();
		this.AddChild(this.cityTileUi);
		this.cityTileUi.SetCityUi(city);
	}

	private void SetTerrainUi(Hex hex)
	{
		this.HideAllPopups();

		this.terrainTileUi = (TerrainTileUi)this.terrainUiScene.Instantiate();
		this.AddChild(this.terrainTileUi);
		this.terrainTileUi.SetHex(hex);
	}

	public override void _ExitTree()
	{
		HexSignals.OnSendHexData -= this.SetTerrainUi;
		HexSignals.OnSendCityData -= this.SetCityUi;
		HexSignals.OnClickOffMap -= this.HideAllPopups;

	}
}
