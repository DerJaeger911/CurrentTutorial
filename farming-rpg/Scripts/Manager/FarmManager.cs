using FarmingRpg.Scripts.Consts;
using FarmingRpg.Scripts.Enums;
using FarmingRpg.Scripts.Manager.AutoLoad;
using FarmingRpg.Scripts.TileData;
using Godot;
using System.Collections.Generic;

namespace FarmingRpg.Scripts.Manager;

public partial class FarmManager : Node
{
    private TileMapLayer tileMapLayer;
    private Dictionary<Vector2I, TileInfo> tileInfo = [];
    private PackedScene cropScene = GD.Load<PackedScene>("res://Scenes/crop.tscn");
    private int tileCount;
    private TileMapDict tileDict = new();
    private Vector2I coords;
    private TileInfo tile;

    public override void _Ready()
    {
        GameSignals.Instance.Connect(SignalConsts.NewDay, new Callable(this, nameof(OnNewDay)));
        GameSignals.Instance.Connect(SignalConsts.HarvestCrop, new Callable(this, nameof(OnHarvestCrop)));


        this.tileMapLayer = this.GetNode<TileMapLayer>("FarmTileMap");

        foreach (var cell in this.tileMapLayer.GetUsedCells())
        {
            this.tileInfo[cell] = new TileInfo();
        }
    }

    private void OnNewDay(int day)
    {
        foreach (var tilePos in this.tileMapLayer.GetUsedCells())
        {
            this.CheckViableTile(tilePos);
            if (this.tile.Watered)
            {
                this.SetTileState(TileTypeEnums.Tilled);
            }
            else if (this.tile.Tilled && this.tile.Crop == null)
            {
                this.SetTileState(TileTypeEnums.Grass);
            }
        }
    }

    private void OnHarvestCrop(Crop crop)
    {
        this.CheckViableTile(crop.TileMapCoords);
        this.SetTileState(TileTypeEnums.Tilled);
    }

    public void TryTillTile(Vector2 playerPosition)
    {
        this.CheckViableTile(playerPosition);

        if (this.tile.Crop != null || this.tile.Tilled)
        {
            return;
        }

        this.SetTileState(TileTypeEnums.Tilled);
    }

    public void TryWaterTile(Vector2 playerPosition)
    {
        this.CheckViableTile(playerPosition);

        if (!this.tile.Tilled) 
        {
            return;
        }

        this.SetTileState(TileTypeEnums.TilledWatered);

        if (this.tile.Crop != null)
        {
            this.tile.Crop.Watered = true;
        }
    }

    public void TrySeedTile(Vector2 playerPosition, CropData cropData)
    {
        this.CheckViableTile(playerPosition);

        if (!this.tile.Tilled || this.tile.Crop != null || GameManager.Instance.OwnedSeeds[cropData] <= 0)
        {
            return;
        }

        var crop = this.cropScene.Instantiate<Crop>();
        this.AddChild(crop);
        crop.GlobalPosition = this.tileMapLayer.MapToLocal(this.coords);

        crop.SetCrop(cropData, this.IsTileWatered(this.coords), this.coords);

        this.tile.Crop = crop;

        GameSignals.Instance.EmitSignal(SignalConsts.ConsumeSeed, cropData);
    }

    public void TryHarvestTile(Vector2 playerPosition)
    {
        this.CheckViableTile(playerPosition);

        if (this.tile?.Crop is { Harvestable: false })
        {
            return;
        }

        GameManager.Instance.HarvestCrop(this.tile.Crop);
        this.tile.Crop = null;
    }

    private bool IsTileWatered(Vector2 position)
    {
        this.CheckViableTile(position);
        return this.tile.Watered;
    }

    private void SetTileState(TileTypeEnums tileType)
    {
        this.tileMapLayer.SetCell(this.coords, 0, this.tileDict.TileAtlasCoords[tileType]);

        switch (tileType)
        {
            case TileTypeEnums.Grass:
                this.tile.Tilled = false;
                this.tile.Watered = false;
                break;
            case TileTypeEnums.Tilled:
                this.tile.Tilled = true;
                this.tile.Watered = false;
                break;
            case TileTypeEnums.TilledWatered:
                this.tile.Tilled = true;
                this.tile.Watered = true;
                break;

        }
    }

    private void CheckViableTile(Vector2 playerPosition)
    {
        this.coords = this.tileMapLayer.LocalToMap(playerPosition);
        this.tileInfo.TryGetValue(this.coords, out this.tile);
    }

    public override void _ExitTree()
    {
        GameSignals.Instance.Disconnect(SignalConsts.NewDay, new Callable(this, nameof(OnNewDay)));
        GameSignals.Instance.Disconnect(SignalConsts.HarvestCrop, new Callable(this, nameof(OnHarvestCrop)));
    }
}
