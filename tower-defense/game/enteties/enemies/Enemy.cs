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
        this.pathFollow.Progress += 50.0f * (float)delta;

        if (this.pathFollow.ProgressRatio >= 0.99f)
        {
            GD.Print(this.UniqueNameInOwner + " got destroyed");
            this.QueueFree();
        }
    }
}