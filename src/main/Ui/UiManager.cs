using Godot;
using System.Security.AccessControl;
using twentyfourtyeight.src.main;
using twentyfourtyeight.src.main.SignalHubs;

public partial class UiManager : Node2D
{
	private PackedScene terrainUiScene;

	private TerrainTileUi terrainTileUi;

	public override void _Ready()
	{
		this.terrainUiScene = ResourceLoader.Load<PackedScene>("res://src/main/Ui/terrain_tile_ui.tscn");
		HexSignals.OnSendHexData += this.SetTerrainUi;
		HexSignals.OnClickOffMap += this.HideAllPopups;
	}

	public void HideAllPopups()
	{
		if (this.terrainTileUi is not null)
		{
			this.terrainTileUi.QueueFree();
			this.terrainTileUi = null;
		}
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
		HexSignals.OnClickOffMap -= this.HideAllPopups;

	}
}
