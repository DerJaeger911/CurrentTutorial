using System.Collections.Generic;
using Godot;

public partial class Tower : Node2D
{
    protected List<Enemy> Enemies { get; private set; } = [];
    private Area2D enemyDetectionArea;
    protected Timer ReloadTimer { get; set; }
    protected Node2D BulletContainer { get; set; }

    [Export]
    private Area2D clickArea;

    protected float ShootDir;

    [Export]
    protected BulletTypeEnums BulletType { get; set; }

    private PackedScene bulletScene = GD.Load<PackedScene>("res://game/bullets/Bullet.tscn");

    public override void _Ready()
    {
        this.enemyDetectionArea = this.GetNode<Area2D>("EnemyDetectionArea");
        this.enemyDetectionArea.AreaEntered += this.OnEnemyDetected;
        this.enemyDetectionArea.AreaExited += this.OnEnemyLost;
        this.BulletContainer = this.GetTree().Root.GetNode<Node2D>("Level/Bullets");

        this.ReloadTimer = this.GetNode<Timer>("ReloadTimer");
        this.ReloadTimer.Timeout += this.OnReloadTimerTimeout;

        this.clickArea.InputEvent += this.OnClickInputArea;
        
    }

    public override void _Process(double delta)
    {

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

    protected void OnReloadTimerTimeout()
    {
        if (this.Enemies.Count > 0)
        {
            this.CreateBullet();
        }
    }

    private void OnClickInputArea(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            EventBus.Instance.EmitSignal(EventBus.SignalName.SelectPlacedTower, this);
        }
    }

    protected virtual void CreateBullet()
    {
        if (this.BulletType == BulletTypeEnums.Single)
        {
            Bullet bullet = this.bulletScene.Instantiate<Bullet>();
            Vector2 dir = Vector2.Down.Rotated(this.ShootDir).Normalized();
            bullet.Setup(this.GlobalPosition + (dir * 16), this.ShootDir, this.BulletType);
            this.BulletContainer.AddChild(bullet);
        }
        if (this.BulletType == BulletTypeEnums.Fire)
        {
            if (this is TowerBlaster towerBlaster)
            {
                towerBlaster.FireAnimation();
            }
            foreach (Enemy enemy in this.GetTree().GetNodesInGroup("Enemies"))
            {
                if (this.GlobalPosition.DistanceTo(enemy.GlobalPosition) < 100f)
                {
                    enemy.Hit();
                }
            }
        }
    }
}