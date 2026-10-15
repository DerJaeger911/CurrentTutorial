using System;
using Godot;

public partial class Enemy : Area2D
{
    [Export]
    private Sprite2D sprite;
    [Export]
    private CollisionShape2D collisionShape;

    private PathFollow2D pathFollow;
    private int health;
    private int speed;

    public void Setup(PathFollow2D pathFollow,EnemyTypeEnums enemyType)
    {
        this.pathFollow = pathFollow;
        this.AreaEntered += this.OnAreaEntered;
        this.health = (int)Data.EnemyData[enemyType]["health"];
        this.speed = (int)Data.EnemyData[enemyType]["speed"];
        this.sprite.Texture = GD.Load<Texture2D>((string)Data.EnemyData[enemyType]["texture"]);
        this.Position += new Vector2(GD.RandRange(-4, 4),GD.RandRange(-4, 4));
    }

    public override void _Process(double delta)
    {
        this.pathFollow.Progress += this.speed * (float)delta;

        if (this.pathFollow.ProgressRatio >= 0.99f)
        {
            Data.Instance.Health--;
            this.QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        this.health--;
        if (this.health <= 0)
        {
            this.QueueFree();
        }
        //Despawn Bullet
        if (area is Bullet bullet)
        {
            bullet.QueueFree();
        }
    }
}