using bullethellcourse.Scripts.Entities;
using bullethellcourse.Scripts.Managers;
using Godot;
using System;

namespace bullethellcourse.Scripts.Pools;

public partial class BossPool : EnemyPoolBase
{
    protected override PackedScene nodeScene => EnemyLoadManager.Instance.GetEnemy(EnemyTypeEnum.Boss);
}
