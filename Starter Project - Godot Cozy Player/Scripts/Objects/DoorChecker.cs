using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;
using System;

public partial class DoorChecker : Area2D
{
    private Vector2I doorCoordinate;

    public Vector2I DoorCoordinate { get => this.doorCoordinate; set => this.doorCoordinate = value; }

    public override void _Ready()
    {
        this.CollisionMask = LayerMask.DoorMask;
        this.BodyEntered += this.OnBodyEntered;
        this.BodyExited += this.OnBodyExited;
    }

    public void Setup(Vector2I position)
    {
        this.DoorCoordinate = position;
        this.Position = new Vector2(position.X * GameConstants.TileSize + GameConstants.TileSize / 2,
            position.Y * GameConstants.TileSize + GameConstants.TileSize / 2);
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("Open door");
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.DoorChanged, this.DoorCoordinate, true);
	}

    private void OnBodyExited(Node body) 
    { 
        GD.Print("Close door");
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.DoorChanged, this.DoorCoordinate, false);
	}

    public override void _ExitTree()
    {
		this.BodyEntered -= this.OnBodyEntered;
		this.BodyExited -= this.OnBodyExited;
	}
}
