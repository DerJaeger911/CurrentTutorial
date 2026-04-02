using Godot;
using System;
using System.Threading.Tasks;

public partial class CharacterVisuals : Sprite2D
{
	private float bobAmount = 0.02f;
	private float bobSpeed = 15.0f;

	private Vector2 baseOffset;
	private float shakeIntensity;
	private float shakedamping = 10;

	public override void _Ready()
	{
		this.baseOffset = this.Offset;
	}

	public void Init(bool faceLeft, Texture2D texture)
	{
		if (faceLeft)
		{
			this.FlipH = true;
		}

		if (texture != null)
		{
			this.Texture = texture;
		}
	}

	public override void _Process(Double delta)
	{
		float time = Time.GetTicksMsec() / 1000.0f;

		float yScale = 1 + (MathF.Sin(time * this.bobSpeed) * this.bobAmount);
		this.Scale = new Vector2(this.Scale.X, yScale);

		if (this.shakeIntensity > 0)
		{
			this.shakeIntensity = Mathf.Lerp(this.shakeIntensity, 0, this.shakedamping * (float)delta);
			this.Offset = this.baseOffset + this.ShakeOffset();
		}
	}

	public async Task DamageVisuals()
	{
		this.Modulate = Colors.Red;
		this.shakeIntensity = 10;
		await this.ToSignal(this.GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
		this.Modulate = Colors.White;
	}

	private Vector2 ShakeOffset()
	{
		float x = (float)GD.RandRange(-this.shakeIntensity, this.shakeIntensity);
		float y = (float)GD.RandRange(-this.shakeIntensity, this.shakeIntensity);
		return new Vector2(x,y);
	}
}
