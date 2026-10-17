using Godot;
using System.Collections.Generic;

public partial class TowerBlaster : Tower
{
	[Export]
	private Node2D particleEmitterGroup;

	public override void _Ready()
	{
		base._Ready();
	}

	public void FireAnimation()
	{
		foreach (GpuParticles2D particles in this.particleEmitterGroup.GetChildren())
		{
			particles.Emitting = true;

		}
	}

	public override void UpgradeTower()
	{
		base.UpgradeTower();
        this.Base.Texture = GD.Load<Texture2D>("res://assets/graphics/towers/blaster/blaster upgrade.png");

	}
}
