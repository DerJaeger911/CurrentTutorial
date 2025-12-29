using Godot;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Entities.Bases;
using RoguelikeCourse.Scripts.Manager.Signals;
using RoguelikeCourse.Scripts.Statics;

public partial class NextLevelPortal : Area2D
{
    
    private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        this.CollisionLayer = LayerMasks.BaseLayer;
        this.CollisionMask = LayerMasks.PlayerLayer;
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
		this.collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
	}

    private void OnBodyEntered(Node body) 
    {
        if (body is not Player player)
        {
            return;
        }
		this.CallDeferred(nameof(ChangeToScene));
		
	}

    private void ChangeToScene()
    {
		this.GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
	}
	public override void _ExitTree()
	{
		GameSignals.Instance.EnemyDefeated -= this.OnDefeatEnemy;
		BodyEntered -= this.OnBodyEntered;
	}
}
