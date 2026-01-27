using Godot;
using System;

public partial class CameraController : SpringArm3D
{
	[Export]
	private float minLimit = -0.8f;
	[Export]
	private float maxLimit = -0.2f;
	[Export]
	private float horizontalAcceleration = 2;
	[Export]
	private float verticalAcceleration = 1;
	[Export]
	private float mouseAcceleration = 0.005f;
	[Export]
	private bool invertY = false;
	[Export]
	private bool invertX = false;

	override public void _Ready()
	{
		base._Ready();
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(Double delta)
    {
        base._Process(delta);
		Vector2 joyDir = Input.GetVector("pan_left", "pan_right", "pan_up", "pan_down");
		Vector2 joyDirAccelerated = joyDir * (float)delta * new Vector2(this.horizontalAcceleration, this.verticalAcceleration);
		this.RotateFromVector(joyDirAccelerated);
	}

	override public void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion mouseMotion)
		{
			var mouseDirAccelerated = -mouseMotion.Relative * this.mouseAcceleration;
			this.RotateFromVector(mouseDirAccelerated);
		}
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}

	private void RotateFromVector(Vector2 dir)
	{
		if(dir.Length() == 0)
		{
			return;
		}
		if(this.invertY)
		{
			dir.Y *= -1;
		}
		if(this.invertX)
		{
			dir.X *= -1;
		}
        this.Rotation = new Vector3(
		Mathf.Clamp(this.Rotation.X + dir.Y, this.minLimit, this.maxLimit),
        this.Rotation.Y + dir.X,
        this.Rotation.Z);
	}
}
