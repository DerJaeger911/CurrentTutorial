using Godot;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Entities.Bases;
using RoguelikeCourse.Scripts.Manager.Signals;

public partial class NextLevelPortal : Area2D
{
    private PackedScene nextLevel;
    private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        GameSignals.Instance.EnemyDefeated += this.OnDefeatEnemy;
        this.Visible = false;
        this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.collisionShape.Disabled = true;
        BodyEntered += this.OnBodyEntered;
    }

    private void OnDefeatEnemy(Enemy enemy)
    {
        if(enemy is not Boss boss)
        {
            return;
        }
        else
        {
			this.CallDeferred(nameof(ActivatePortal));
		}
    }

	private void ActivatePortal()
	{
		this.Visible = true;
		this.collisionShape.SetDeferred(
			CollisionShape2D.PropertyName.Disabled,
			false
		);
	}

	public override void _ExitTree()
    {
		GameSignals.Instance.EnemyDefeated -= this.OnDefeatEnemy;
		BodyEntered -= this.OnBodyEntered;
	}

    private void OnBodyEntered(Node body) 
    {
        if (body is not Player player)
        {
            return;
        }

        this.GetTree().ChangeSceneToPacked(this.nextLevel);
    }
}
