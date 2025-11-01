using FarmingRpg.Scripts;
using FarmingRpg.Scripts.Enums;
using Godot;
using System.Collections.Generic;


public partial class FarmManager : Node
{
    private TileMapLayer tileMapLayer;

    private Dictionary<Vector2I, TileInfo> tileInfo;

    private PackedScene packedScene = GD.Load<PackedScene>("res://Scenes/crop.tscn");

    private Dictionary<TileTypeEnums, Vector2I> tileAtlasCoords = new() {
        { TileTypeEnums.Grass, new Vector2I(0,0) },
        { TileTypeEnums.Tilled, new Vector2I(1,0)},
        { TileTypeEnums.TilledWater, new Vector2I(0,1)}
    };



    public override void _Ready()
    {
        this.tileMapLayer = this.GetNode<TileMapLayer>("FarmTileMap");
    }
}
