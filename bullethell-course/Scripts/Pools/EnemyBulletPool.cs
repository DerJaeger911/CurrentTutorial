using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Entities;
using bullethellcourse.Scripts.Managers;
using Godot;

namespace bullethellcourse.Scripts.Pools;

public partial class EnemyBulletPool : BulletPool
{
    protected override PackedScene nodeScene => BulletLoadManager.Instance.GetBullet(BulletTypeEnum.Fire);
}
