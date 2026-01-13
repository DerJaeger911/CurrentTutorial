using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;

public partial class Layers : Node2D
{
	private TileMapLayer waterLayer;
	private TileMapLayer hillsLayer;	

    public override void _Ready()
    {
		this.ZIndex = -2;

        this.waterLayer = this.GetNode<TileMapLayer>("WaterLayer");
		this.hillsLayer = this.GetNode<TileMapLayer>("HillsLayer");

		var waterTileSet = this.waterLayer.TileSet;
		var hillsTileSet = this.hillsLayer.TileSet;

		waterTileSet.SetPhysicsLayerCollisionLayer(0, LayerMask.TerainLayer);
		hillsTileSet.SetPhysicsLayerCollisionLayer(0, LayerMask.TerainLayer);


	}
}
