using System.Collections.Generic;
using Godot;

public partial class Tower : Node2D
{
    protected List<Enemy> Enemies { get; private set; } = [];
    private Area2D enemyDetectionArea;
    protected Timer ReloadTimer { get; set; }
    private Node2D bulletContainer;

    protected Sprite2D turret;

    private PackedScene bulletScene = GD.Load<PackedScene>("res://game/Bullets/Bullet.tscn");

    public override void _Ready()
    {
        this.enemyDetectionArea = this.GetNode<Area2D>("EnemyDetectionArea");
        this.enemyDetectionArea.AreaEntered += this.OnEnemyDetected;
        this.enemyDetectionArea.AreaExited += this.OnEnemyLost;
        this.bulletContainer = this.GetTree().Root.GetNode<Node2D>("Level/Bullets");
        this.turret = this.GetNode<Sprite2D>("Turret");
        this.ReloadTimer = this.GetNode<Timer>("ReloadTimer");
        this.ReloadTimer.Timeout += this.OnReloadTimerTimeout;

        GD.Print(this.GetTree().Root.Name);
    }

    public override void _Process(double delta)
    {
        GD.Print(this.Enemies);
    }

    private void OnEnemyDetected(Area2D area)
    {
        if (area is Enemy enemy && !this.Enemies.Contains(enemy))
        {
            this.Enemies.Add(enemy);
        }
    }

    private void OnEnemyLost(Area2D area)
    {
        if (area is Enemy enemy && this.Enemies.Contains(enemy))
        {
            _ = this.Enemies.Remove(enemy);
        }
    }

    private void OnReloadTimerTimeout()
    {
        if (this.Enemies.Count > 0)
        {
            this.CreateBullet();
        }

    }

    private void CreateBullet()
    {
        Bullet bullet = this.bulletScene.Instantiate<Bullet>();
        Vector2 dir = Vector2.Down.Rotated(this.turret.Rotation).Normalized();
        bullet.Setup(this.GlobalPosition + (dir * 16), this.turret.Rotation, BulletTypeEnums.Single);
        this.bulletContainer.AddChild(bullet);
    }
}
