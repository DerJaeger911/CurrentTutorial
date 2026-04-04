using Godot;

namespace Metroidvania.game.utility;

public static class FX
{
	private static readonly PackedScene explosionScene = GD.Load<PackedScene>("res://game/bullets/Explosion.tscn");
	private static readonly PackedScene bulletScene = GD.Load<PackedScene>("res://game/bullets/Bullet.tscn");


	public static void SpawnExplosion(Node2D caller)
	{
		var explosion = explosionScene.Instantiate<Explosion>();
		explosion.Position = caller.Position;
		var bulletContainer = GetBulletContainer(caller);
		bulletContainer.CallDeferred(Node.MethodName.AddChild, explosion);
		explosion.Setup(caller);
	}

	public static void SpawnBullet(Node2D caller,Marker2D spawner, Vector2 direction)
	{
		var bullet = bulletScene.Instantiate<Bullet>();
		var bulletContainer = GetBulletContainer(caller);
		bulletContainer.AddChild(bullet);
		bullet.Setup(caller, spawner.GlobalPosition, direction);
	}

	private static Node GetBulletContainer(Node2D caller)
	{
		Node2D bulletContainer = caller.GetParent().GetParent().GetNode<Node2D>("Bullets");
		if (bulletContainer == null)
		{
			caller.GetParent().GetParent().AddChild(new Node2D() { Name = "Bullets" });
			GD.Print("Bim");
		}
		return bulletContainer;
	}
}
