using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;

public partial class Layers : Node2D
{
	private TileMapLayer waterLayer;
	private TileMapLayer hillsLayer;
	private TileMapLayer soilLayer;

    public override void _Ready()
    {
		this.ZIndex = -2;

        this.waterLayer = this.GetNode<TileMapLayer>("WaterLayer");
		this.hillsLayer = this.GetNode<TileMapLayer>("HillsLayer");
		this.soilLayer = this.GetNode<TileMapLayer>("SoilLayer");

		TileSet waterTileSet = this.waterLayer.TileSet;
		TileSet hillsTileSet = this.hillsLayer.TileSet;

		waterTileSet.SetPhysicsLayerCollisionLayer(0, LayerMask.TerainLayer);
		hillsTileSet.SetPhysicsLayerCollisionLayer(0, LayerMask.TerainLayer);
	}

	public void AddSoil(Vector2I soilCoords)
	{
		this.soilLayer.SetCellsTerrainConnect([soilCoords], 0, 0);
	}
}
