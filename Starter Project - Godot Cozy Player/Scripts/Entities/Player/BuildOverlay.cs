using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.AutoLoads.Managers;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Entities.Player;
using Dcozysandbox.Scripts.Enums;
using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;

public partial class BuildOverlay : Node2D
{
	private Vector2I currentGridPosition;
	private Player player;
	[Export]
	private Vector2I startGridPosition = new Vector2I(2, 0);
	private ObjectEnum currentBuildObject = ObjectEnum.Walls;
	private Sprite2D previewSprite;
	private double moveTimer = 0.0;
	private const double MoveDelay = 0.15;

	public override void _Ready()
    {
		this.player = this.GetNode<Player>("../../Objects/Player");
		this.previewSprite = this.GetNode<Sprite2D>("PreviewSprite");
    }

    public override void _Input(InputEvent @event)
    {
        if (this.player.IsBuilding)
		{
			if (Input.IsActionJustPressed("action"))
			{
				if(PlayerResourceManager.Instance.CheckResource(ResourceEnum.Wood, 1))
				{
					SignalBus.Instance.EmitSignal(SignalBus.SignalName.Build, this.currentGridPosition, (int)this.currentBuildObject);
					PlayerResourceManager.Instance.SubtractResource(ResourceEnum.Wood, 1);
				}
			}

			if (Input.IsActionJustPressed("ui_text_backspace"))
			{
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.DeleteBuild, this.currentGridPosition);
			}
			
			float toggleDirection = Input.GetAxis("tool_backward", "tool_forward");
			if (!Mathf.IsZeroApprox(toggleDirection))
			{
				int enumSize = System.Enum.GetValues(typeof(ObjectEnum)).Length;
				int nextIndex = Mathf.PosMod((int)this.currentBuildObject + (int)toggleDirection, enumSize);
				this.currentBuildObject = (ObjectEnum)nextIndex;
				this.previewSprite.Frame = nextIndex;
				GD.Print(nextIndex);
			}
		}

    }

    public override void _Process(Double delta)
    {
		if (this.player.IsBuilding)
		{
            this.moveTimer += delta;

			if (this.moveTimer >= MoveDelay)
			{
				Vector2I direction = (Vector2I)Input.GetVector("left", "right", "up", "down");

				if (direction != Vector2I.Zero)
				{
					this.currentGridPosition += direction;
					this.Position = this.currentGridPosition * GameConstants.TileSize;
                    this.moveTimer = 0.0;
				}
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
