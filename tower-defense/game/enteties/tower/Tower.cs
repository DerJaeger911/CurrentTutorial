using System.Collections.Generic;
using Godot;

public partial class Tower : Node2D
{
    [Export]
    protected TowerTypeEnums TowerType { get; set; }
    [Export]
    protected Sprite2D Base { get; set; }
    [Export]
    private Area2D clickArea { get; set; }
    [Export]
    protected Timer ReloadTimer { get; set; }
    [Export]
    protected Timer DelayTimer { get; set; }
    [Export]
    private UpgradeMenu upgradeMenu;

    protected BulletTypeEnums BulletType { get; set; }
    protected List<Enemy> Enemies { get; private set; } = [];
    private Area2D enemyDetectionArea;

    protected Node2D BulletContainer { get; set; }

    protected int Cost { get; set; }
    protected int UpgradeCost { get; set; }



    protected bool Upgraded { get; set; }

    public Level LevelInstance { get; set; }



    protected float ShootDir;



    private PackedScene bulletScene = GD.Load<PackedScene>("res://game/bullets/Bullet.tscn");
    private PackedScene explosionScene = GD.Load<PackedScene>("res://game/bullets/Explosion.tscn");

    public override void _Ready()
    {
        this.enemyDetectionArea = this.GetNode<Area2D>("EnemyDetectionArea");
        this.enemyDetectionArea.AreaEntered += this.OnEnemyDetected;
        this.enemyDetectionArea.AreaExited += this.OnEnemyLost;
        this.BulletContainer = this.GetTree().Root.GetNode<Node2D>("Level/Bullets");

        this.ReloadTimer.Timeout += this.OnReloadTimerTimeout;

        this.clickArea.InputEvent += this.OnClickInputArea;
        EventBus.Instance.SelectPlacedTower += this.OnSelectedPlacedTower;
    }

    public void Setup()
    {
        this.ReloadTimer.WaitTime = (float)Data.TowerData[this.TowerType]["reload_time"];
        this.BulletType = (BulletTypeEnums)Data.TowerData[this.TowerType]["bullet"];
        this.Cost = (int)Data.TowerData[this.TowerType]["cost"];
        this.UpgradeCost = (int)Data.TowerData[this.TowerType]["upgrade_cost"];


        GD.Print(this.TowerType);
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

    protected virtual void OnReloadTimerTimeout()
    {
        if (this.Enemies.Count > 0)
        {
            this.CreateBullet();
        }
    }

    private void OnClickInputArea(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && 
        mouseEvent.ButtonIndex == MouseButton.Left && 
        mouseEvent.Pressed)
        {
            if (this.DelayTimer.TimeLeft == 0)
            {
                EventBus.Instance.EmitSignal(EventBus.SignalName.SelectPlacedTower);
                this.upgradeMenu.Reveal();
            }
        }
    }

    protected virtual void OnSelectedPlacedTower()
    {
        this.LevelInstance.CurrentSelectedTower = this;
    }

    protected virtual void CreateBullet(Vector2 pos = default)
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
        if (this.BulletType == BulletTypeEnums.MortarExplosion)
        {
            Explosion explosion = this.explosionScene.Instantiate<Explosion>();
            explosion.Setup(pos);
            this.BulletContainer.AddChild(explosion);
        }
    }

    public virtual void UpgradeTower()
    {
        this.Upgraded = true;
    }

    public void DeleteTower()
    {
        var returnMoney = !this.Upgraded ? this.Cost : (this.Cost + this.UpgradeCost)
        Data.Instance.Money += returnMoney
        this.QueueFree();
    }
}