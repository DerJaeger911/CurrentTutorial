using bullethellcourse.Scripts.Bullets;
using Godot;

namespace bullethellcourse.Scripts.Entities;

internal partial class Enemy : Entity
{
    private Entity enemyEntity;

	private EnemyBulletPool enemyBulletPool = new();

	protected override BulletPool bulletPool => this.enemyBulletPool;

	protected override EntityTypeEnum EntityType => EntityTypeEnum.Enemy;

	protected override Vector2 BulletDirection()
    {
        var shootDirection = this.GlobalPosition.DirectionTo(this.enemyEntity.GlobalPosition);
        return shootDirection;
    }
}
