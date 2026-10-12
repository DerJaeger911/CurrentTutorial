using Godot;

public partial class Enemy : Area2D
{
    private PathFollow2D pathFollow;

    [Export]
    private int health = 5;

    public void Setup(PathFollow2D pathFollow)
    {
        this.pathFollow = pathFollow;
        this.AreaEntered += this.OnAreaEntered;
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