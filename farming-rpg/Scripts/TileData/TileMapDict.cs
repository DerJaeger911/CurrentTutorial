using FarmingRpg.Scripts.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingRpg.Scripts.TileData
{
    internal class TileMapDict
    {
        public Dictionary<TileTypeEnums, Vector2I> TileAtlasCoords = new() {
        { TileTypeEnums.Grass, new Vector2I(0,0) },
        { TileTypeEnums.Tilled, new Vector2I(1,0)},
        { TileTypeEnums.TilledWatered, new Vector2I(0,1)}
    };
    }
}
