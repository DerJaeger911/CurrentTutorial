using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;
using System;

public partial class WallLayer : TileMapLayer
{
    public override void _Ready()
    {
        base._Ready();
        this.TileSet.SetPhysicsLayerCollisionLayer(0, LayerMask.TerainLayer);
	}
}
