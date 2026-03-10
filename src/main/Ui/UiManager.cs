using Godot;
using System.Security.AccessControl;
using twentyfourtyeight.src.main;
using twentyfourtyeight.src.main.SignalHubs;

public partial class UiManager : Node2D
{
	private PackedScene terrainUiScene;
	private PackedScene cityUiScene;
	private PackedScene unitUiScene;


	private TerrainTileUi terrainTileUi;
	private CityUi cityTileUi;
	private UnitUi unitUi;

	public override void _Ready()
	{
		this.terrainUiScene = ResourceLoader.Load<PackedScene>("res://src/main/Ui/terrain_tile_ui.tscn");
		this.cityUiScene = ResourceLoader.Load<PackedScene>("res://src/main/Ui/CityUi.tscn");
        this.unitUiScene = ResourceLoader.Load<PackedScene>("res://src/main/Ui/UnitUi.tscn");


        HexSignals.OnSendHexData += this.SetTerrainUi;
		HexSignals.OnSendCityData += this.SetCityUi;
		HexSignals.OnClickOffMap += this.HideAllPopups;
		UISignals.UnitClicked += this.SetUnitUi;

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
        if (this.unitUi is not null)
        {
            this.unitUi.QueueFree();
            this.unitUi = null;
        }
    }

	private void SetCityUi(City city)
	{
		this.HideAllPopups();

		this.cityTileUi = (CityUi)this.cityUiScene.Instantiate();
		this.AddChild(this.cityTileUi);
		this.cityTileUi.SetCityUi(city);
	}

	private void SetUnitUi(Unit unit)
	{
		this.HideAllPopups();

		this.unitUi = (UnitUi)this.unitUiScene.Instantiate();
		this.AddChild(this.unitUi);
		this.unitUi.SetUnit(unit);
		this.unitUi.Refresh();
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
		UISignals.UnitClicked -= this.SetUnitUi;
	}
}
