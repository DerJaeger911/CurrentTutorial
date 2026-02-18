using Godot;
using System;

public partial class Camera2d : Camera2D
{
	[Export]
	private int cameraVelocity = 50;

	[Export]
	private float zoomSpeed = 1;

	private float mouseWheelSensitivity = 0.1f;

	private float leftBound, rightBound, topBound, bottomBound;

	private HexTileMap hexTileMap;

	private Vector2 targetZoom;


	override public void _Ready()
	{
		this.hexTileMap = this.GetNode<HexTileMap>("../HexTileMap");
		this.leftBound = this.ToGlobal(this.hexTileMap.MapToLocal(new Vector2I(0, 0))).X + 100;
		this.rightBound = this.ToGlobal(this.hexTileMap.MapToLocal(new Vector2I(this.hexTileMap.Width, 0))).X - 100;
		this.topBound = this.ToGlobal(this.hexTileMap.MapToLocal(new Vector2I(0, 0))).Y + 50;
		this.bottomBound = this.ToGlobal(this.hexTileMap.MapToLocal(new Vector2I(0, this.hexTileMap.Height))).Y - 50;
		this.targetZoom = this.Zoom;

		this.Position = new Vector2(
			(this.leftBound + this.rightBound) / 2,
			(this.topBound + this.bottomBound) / 2
		);
	}

	public override void _PhysicsProcess(Double delta)
	{
		Vector2 inputDir = (Input.GetVector("map_left", "map_right", "map_up", "map_down"));
		if (inputDir != Vector2.Zero)
		{
			float zoomFactor = 1.0f + (this.Zoom.X * 0.5f);
			Vector2 nextPosition = this.Position + inputDir * this.cameraVelocity / zoomFactor;
			nextPosition.X = Mathf.Clamp(nextPosition.X, this.leftBound, this.rightBound);
			nextPosition.Y = Mathf.Clamp(nextPosition.Y, this.topBound, this.bottomBound);

			this.Position = nextPosition;
		}

		float zoomInput = Input.GetAxis("map_zoom_out", "map_zoom_in");

		if (zoomInput != 0)
		{
			this.ZoomInAndOut(zoomInput * (float)delta * this.zoomSpeed);
		}

		if (Input.IsActionJustPressed("mouse_zoom_in")) 
		{ 
			this.ZoomInAndOut(1 * this.zoomSpeed * this.mouseWheelSensitivity); 
		}
		if (Input.IsActionJustPressed("mouse_zoom_out")) 
		{ 
			this.ZoomInAndOut(-1 * this.zoomSpeed * this.mouseWheelSensitivity); 
		}

		this.Zoom = this.targetZoom;
	}

	private void ZoomInAndOut(float zoomInput)
	{
		this.targetZoom += Vector2.One * zoomInput * this.zoomSpeed;
		this.targetZoom = this.targetZoom.Clamp(new Vector2(0.1f, 0.1f), new Vector2(3.0f, 3.0f));
		
	}
}
