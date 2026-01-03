using bullethellcourse.Scripts.Bullets;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethellcourse.Scripts.Managers;

internal class BulletLoadManager
{
	private static BulletLoadManager instance = new();
	public static BulletLoadManager Instance => instance;

	private Dictionary<BulletTypeEnum, PackedScene> bulletScenes = new();

	private BulletLoadManager()
	{
		bulletScenes[BulletTypeEnum.Arrow] = GD.Load<PackedScene>("res://Scenes/bullet.tscn");
		bulletScenes[BulletTypeEnum.Fire] = GD.Load<PackedScene>("res://Scenes/enemy_bullet.tscn");
	}

	public PackedScene GetBullet(BulletTypeEnum key)
	{
		return bulletScenes.TryGetValue(key, out var scene) ? scene : null;
	}
}
