using Godot;

public partial class Bullet : Area2D
{
    private Vector2 direction;
    private float speed = 200f;
    private BulletTypeEnums bulletType;

    public void Setup(Vector2 pos, float angle, BulletTypeEnums bulletType)
    {
        this.Position = pos;
        this.direction = Vector2.Down.Rotated(angle);
        this.Rotation = angle;
        this.bulletType = bulletType;
    }

    public override void _Process(double delta)
    {
        this.Position += this.direction * this.speed * (float)delta;
    }
}
