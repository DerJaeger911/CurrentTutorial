using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Godot;

public partial class Enemy : Area2D
{
    [Export]
    private Sprite2D sprite;
    [Export]
    private CollisionShape2D collisionShape;
    [Export]
    private AudioStreamPlayer2D hitSound;

    private PathFollow2D pathFollow;
    private int health;
    private int speed;

    public void Setup(PathFollow2D pathFollow, EnemyTypeEnums enemyType)
    {
        this.pathFollow = pathFollow;
        this.AreaEntered += this.OnAreaEntered;
        this.health = (int)Data.EnemyData[enemyType]["health"];
        this.speed = (int)Data.EnemyData[enemyType]["speed"];
        this.sprite.Texture = GD.Load<Texture2D>((string)Data.EnemyData[enemyType]["texture"]);
        this.Position = new Vector2(GD.RandRange(-4, 4), GD.RandRange(-4, 4));
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

    private void Flash()
    {
        if (this.sprite.Material is ShaderMaterial shaderMat)
        {
            Tween tween = this.CreateTween();
            tween.TweenProperty(shaderMat, "shader_parameter/Progress", 1.0f, 0.2f);
            tween.TweenProperty(shaderMat, "shader_parameter/Progress", 0.0f, 0.2f);
        }
    }

    public void Hit()
    {
        this.health--;
        this.Flash();
        this.hitSound.Play();
        if (this.health <= 0)
        {
            this.QueueFree();
            Data.Instance.Money++;
        }

    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Bullet bullet)
        {
            bullet.QueueFree();
        }
        this.Hit();
    }
}