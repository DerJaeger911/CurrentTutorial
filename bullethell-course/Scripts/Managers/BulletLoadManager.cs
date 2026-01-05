using bullethellcourse.Scripts.Bullets;
using Godot;
using System.Collections.Generic;

namespace bullethellcourse.Scripts.Managers;

internal class BulletLoadManager
{
	private static BulletLoadManager instance = new();
	public static BulletLoadManager Instance => instance;

	private Dictionary<BulletTypeEnum, PackedScene> bulletScenes = new();

	private BulletLoadManager()
	{
        this.bulletScenes[BulletTypeEnum.Arrow] = GD.Load<PackedScene>("res://Scenes/bullet.tscn");
        this.bulletScenes[BulletTypeEnum.Fire] = GD.Load<PackedScene>("res://Scenes/enemy_bullet.tscn");
		this.bulletScenes[BulletTypeEnum.Star] = GD.Load<PackedScene>("res://Scenes/boss_bullet.tscn");
	}

	public PackedScene GetBullet(BulletTypeEnum key)
	{
		return this.bulletScenes.TryGetValue(key, out var scene) ? scene : null;
	}
}
