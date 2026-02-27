using Godot;
using System;

public partial class UiManager : Node2D
{
	private PackedScene terrainUiScene;

	public override void _Ready()
	{
		this.terrainUiScene = ResourceLoader.Load<PackedScene>("TerrainTileUi.cs");
	}
}
