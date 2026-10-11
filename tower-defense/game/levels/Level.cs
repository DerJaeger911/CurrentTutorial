using Godot;

public partial class Level : Node2D
{
    private PackedScene enemyScene = GD.Load<PackedScene>("res://game/enteties/enemies/Enemy.tscn");
    private PathFollow2D pathFollow = new();
    private Path2D path;
    private Enemy enemy;

    public override void _Ready()
    {
        this.path = this.GetNode<Path2D>("Path2D");
        this.path.AddChild(this.pathFollow);

        this.enemy = this.enemyScene.Instantiate<Enemy>();
        this.enemy.Setup(this.pathFollow);

        this.pathFollow.AddChild(this.enemy);
    }
}