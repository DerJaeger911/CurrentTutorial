using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Pools;
using Godot;

namespace bullethellcourse.Scripts.Entities;

internal partial class Boss : Enemy
{
	protected override BulletTypeEnum BulletType => BulletTypeEnum.Star;

	private BossBulletPool bossBulletPool = new();

	protected override BulletPool BulletPool => this.bossBulletPool;
}
