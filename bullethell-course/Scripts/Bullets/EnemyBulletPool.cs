using bullethellcourse.Scripts.Managers;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethellcourse.Scripts.Bullets;

public partial class EnemyBulletPool : BulletPool
{
    protected override PackedScene nodeScene => BulletLoadManager.Instance.GetBullet(BulletTypeEnum.Fire);
}
