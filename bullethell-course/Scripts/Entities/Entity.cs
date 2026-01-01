using bullethellcourse.Scripts.Bullets;
using Godot;

namespace bullethellcourse.Scripts.Entities;

public partial class Entity : CharacterBody2D
{
	[Export]
	protected double shootRate = 0.1f;

	protected double lastShootTime;
	
	protected Node2D muzzle;

	protected PackedScene bulletScene;

    public override void _Ready()
    {
		this.muzzle = this.GetNode<Node2D>("Muzzle");

		this.bulletScene = GD.Load<PackedScene>("res://Scenes/bullet.tscn");
	}

	protected void Shoot(BulletTypeEnum bulletType)
	{
		this.lastShootTime = Time.GetUnixTimeFromSystem();

		Bullet bullet = this.bulletScene.Instantiate<Bullet>();
		bullet.Init(bulletType);
		this.GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, bullet);
		bullet.GlobalPosition = this.muzzle.GlobalPosition;

		var nmousePosition = this.GetGlobalMousePosition();
	}
}
