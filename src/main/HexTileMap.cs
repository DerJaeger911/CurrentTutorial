 using Godot;
using System;

public partial class HexTileMap : Node2D
{
	[Export]
	private int width = 100;
	[Export]
	private int height = 60;

	private TileMapLayer baseLayer, borderLayer, overlayLayer;

	override public void _Ready()
	{
		this.baseLayer = this.GetNode<TileMapLayer>("BaseLayer");
		this.borderLayer = this.GetNode<TileMapLayer>("HexBordersLayer");
		this.overlayLayer = this.GetNode<TileMapLayer>("SelectionOverlayLayer");
		this.GenerateTerrain();
	}

	public void GenerateTerrain() 
	{
		for (int i = 0; i < this.width; i++)
		{
			for (int j = 0; j < this.height; j++)
			{
				this.baseLayer.SetCell(new Vector2I(i, j), 0, new Vector2I(0, 0));
				this.borderLayer.SetCell(new Vector2I(i, j), 0, new Vector2I(0, 0));
			}
		}
	}
}
