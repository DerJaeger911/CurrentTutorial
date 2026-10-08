using Godot;
using System;

public partial class Crosshair : Sprite2D
{
	public void Update(Vector2 direction, int distance, bool isDucking)
	{
		
		float offsetY = isDucking ? 6f : -2f;

		this.Position = direction * distance + new Vector2(0, offsetY);
	}
}
