using Dcozysandbox.Scripts.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class BuildObject : StaticBody2D
{
	private ObjectEnum buildObject;
	private Sprite2D buildSprite;
	private CollisionShape2D collisionShape;
	private bool isSetup;

	private readonly IReadOnlyDictionary<ObjectEnum, Vector2I> collisionSizes =
		new Dictionary<ObjectEnum, Vector2I>
		{
		{ ObjectEnum.Plant, new Vector2I(8, 15)},
		{ ObjectEnum.Shelf, new Vector2I(29, 11)},
		{ ObjectEnum.Table, new Vector2I(29, 11)},
		{ ObjectEnum.Bed, new Vector2I(16, 12)},
		{ ObjectEnum.Carpet, new Vector2I(32, 32)},
		}.AsReadOnly();

	public override void _Ready()
	{
        this.buildSprite = this.GetNode<Sprite2D>("Sprite2D");
		this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");

		if (this.isSetup)
		{
			this.Apply();
		}
	}

	public void Setup(ObjectEnum buildObject)
	{	
		this.buildObject = buildObject;
		this.isSetup = true;

		if (this.buildSprite != null)
		{
			this.Apply();
		}
	}

	private void Apply()
	{
		this.buildSprite.Frame = (int)this.buildObject;
		RectangleShape2D collisionShape = new RectangleShape2D();
		collisionShape.Size = this.collisionSizes[this.buildObject];
		this.collisionShape.Shape = collisionShape;
		if(this.buildObject == ObjectEnum.Carpet)
		{
			this.collisionShape.Disabled = true;
			
		}
	}

}
