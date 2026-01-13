using Godot;
using System;

public partial class SimpleObject : StaticBody2D
{
	private int size;
	private int style;

	private Sprite2D simpleSprite;
    private CollisionShape2D collisionShape; 

    public override void _Ready()
    {
        this.simpleSprite = this.GetNode<Sprite2D>("Sprite");
        this.size = GD.RandRange(0, this.simpleSprite.Hframes - 1);
        this.style = GD.RandRange(0, 1);
		this.simpleSprite.FrameCoords = new Vector2I(this.size, this.style);
        this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        if(this.size < 2)
        {
            this.collisionShape.Disabled = true;
            this.ZIndex = -1;
        }
    }
}
