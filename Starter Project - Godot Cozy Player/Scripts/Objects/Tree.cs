using Godot;
using System;

public partial class Tree : StaticBody2D
{
	[Export]
	private int health = 5;
	
	private Sprite2D treeSprite;
	private Sprite2D stump;
	private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        this.treeSprite = this.GetNode<Sprite2D>("Sprite");
		this.stump = this.GetNode<Sprite2D>("Stump");
		this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
		int[] frames = { 0, 1 };
		this.treeSprite.Frame = frames[(int)(GD.Randi() % (uint)frames.Length)];
		//Because "this.treeSprite.Material.ResourceLocalToScene = true;" not working
		if (this.treeSprite.Material != null)
		{
			this.treeSprite.Material = (Material)this.treeSprite.Material.Duplicate();
		}
	}

	public void Flash()
	{
		Tween tween = this.GetTree().CreateTween();
		tween.TweenProperty(this.treeSprite.Material, "shader_parameter/progress", 1.0, 0.2);
		tween.TweenProperty(this.treeSprite.Material, "shader_parameter/progress", 0.0, 0.4);
		this.health--;
		if (this.health <= 0)
		{
			this.stump.Show();
			this.treeSprite.Hide();
			this.collisionShape.Position = new Vector2(0, 10);
			this.collisionShape.Scale = new Vector2(1, 0.4f);
		}
	}
}
