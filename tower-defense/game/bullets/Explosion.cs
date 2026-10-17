using Godot;

public partial class Explosion : Sprite2D
{
	[Export]
	private AnimationPlayer animationPlayer;
	public void Setup(Vector2 pos)
	{
		this.GlobalPosition = pos;
		this.animationPlayer.Play("explosion");
	}

	public void HitEnemies()
	{
		foreach (Enemy enemy in this.GetTree().GetNodesInGroup("Enemies"))
		{
			if (this.GlobalPosition.DistanceTo(enemy.GlobalPosition) < 30)
			{
				enemy.Hit();
			}
		}
	}
}
