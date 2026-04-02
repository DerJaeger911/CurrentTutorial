using Godot;
using System;

public partial class Background : TextureRect
{
	[Export]
	private float scrollSpeed = 50;
	[Export]
	private float extends = 1024;
	[Export]
	private Gradient colorLerp;

	private Vector2 startPos;

	override public void _Ready()
	{
		this.startPos = this.Position;
	}

	public override void _Process(double delta)
	{
		Vector2 currentPos = this.Position;

		currentPos.X -= this.scrollSpeed * (float)delta;

		this.Position = currentPos;

		if (Mathf.Abs(this.Position.X - this.startPos.X) >= this.extends)
		{
			this.Position = this.startPos;
		}

		GD.Print(this.Position);
	}
}
