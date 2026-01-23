using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Entities.Player;
using Dcozysandbox.Scripts.Enums;
using Godot;
using System;

public partial class BuildOverlay : Node2D
{
	private Vector2I currentGridPosition;
	private Player player;
	[Export]
	private Vector2I startGridPosition = new Vector2I(2, 0);
	private ObjectEnum currentBuildObject = ObjectEnum.Walls;

    public override void _Ready()
    {
		this.player = this.GetNode<Player>("../../Objects/Player");
    }

    public override void _Input(InputEvent @event)
    {
        if (this.player.IsBuilding)
		{
			Vector2I direction = (Vector2I)Input.GetVector("left", "right", "up", "down");
			this.currentGridPosition += direction;
			this.Position = this.currentGridPosition * GameConstants.TileSize;

			if (Input.IsActionJustPressed("action"))
			{
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.Build, this.currentGridPosition, (int)this.currentBuildObject);
			}

			if (Input.IsActionJustPressed("ui_text_backspace"))
			{
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.DeleteBuild, this.currentGridPosition);
			}
		}

    }

	public void Reveal(Vector2 worldPosition)
	{
		this.Visible = true;

		this.currentGridPosition = new Vector2I(
			Mathf.FloorToInt(worldPosition.X / GameConstants.TileSize),
			Mathf.FloorToInt(worldPosition.Y / GameConstants.TileSize)
		) + this.startGridPosition;

		this.GlobalPosition = (Vector2)this.currentGridPosition * GameConstants.TileSize;

		GD.Print($"Build Mode Aktiv - Grid: {this.currentGridPosition}");
	}

	public void UnReveal()
	{
		this.Visible = false;
	}
}
