using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Managers;
using Godot;

namespace bullethellcourse.Scripts.Pools;

public partial class BossBulletPool : BulletPool
{
	protected override PackedScene nodeScene => BulletLoadManager.Instance.GetBullet(BulletTypeEnum.Star);
}
