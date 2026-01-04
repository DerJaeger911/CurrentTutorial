using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Managers;
using Godot;
using System;
using System.Collections.Generic;

namespace bullethellcourse.Scripts.Pools;

public partial class PlayerBulletPool : BulletPool
{
	protected override PackedScene nodeScene => BulletLoadManager.Instance.GetBullet(BulletTypeEnum.Arrow);
}
