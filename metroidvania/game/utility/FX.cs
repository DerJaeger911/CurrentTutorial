using Godot;

namespace Metroidvania.game.utility;

public static class FX
{
	private const int muzzleOffset = 16;

	private static readonly PackedScene explosionScene = GD.Load<PackedScene>("res://game/bullets/Explosion.tscn");
	private static readonly PackedScene bulletScene = GD.Load<PackedScene>("res://game/bullets/Bullet.tscn");


	public static void SpawnExplosion(Node2D caller)
	{
		var explosion = explosionScene.Instantiate<Explosion>();
		explosion.Position = caller.Position;
		var bulletContainer = ContainerFinderHelper.GetContainer<Level>(caller, "Bullets");
		bulletContainer.CallDeferred(Node.MethodName.AddChild, explosion);
		explosion.Setup(caller);
	}

	public static void SpawnBullet(Node2D caller, Marker2D spawner, Vector2 direction, bool isDucking, GunEnums gunType, GpuParticles2D shotgunParticles = null)
	{
		float offsetY = isDucking ? 8f : 0f;

		Vector2 spawnOffset = spawner.GlobalPosition + new Vector2(0, offsetY) + direction * muzzleOffset;

		if (gunType == GunEnums.Shotgun)
		{
			Shotgun shotgun = new Shotgun();
			shotgun.EmitParticles(direction, spawnOffset, shotgunParticles);
			shotgun.OnShotgunHit(caller, direction);
			
		}
		else
		{
			var bullet = bulletScene.Instantiate<Bullet>();
			var bulletContainer = ContainerFinderHelper.GetContainer<Level>(caller, "Bullets");
			bulletContainer.AddChild(bullet);
			bullet.Setup(caller, direction, gunType, spawnOffset);
			Level soundContainer = (Level)ContainerFinderHelper.GetParent<Level>(caller);
			soundContainer.GunSound.Play();
		}
	}
}
