using Godot;

public partial class Enemy : Area2D
{
    private PathFollow2D pathFollow;

    public void Setup(PathFollow2D pathFollow)
    {
        this.pathFollow = pathFollow;
    }

    public override void _Process(double delta)
    {
        this.pathFollow.Progress += 2;
    }
}
