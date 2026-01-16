using Dcozysandbox.Scripts.Textures;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Tree : StaticBody2D
{
	[Export]
	private int health = 5;

	private Sprite2D treeSprite;
	private Sprite2D stump;
	private CollisionShape2D collisionShape;
	private Node2D applePositions;
	private List<Marker2D> appleMarker;
	private Node2D apple;

	public override void _Ready()
	{
		this.treeSprite = this.GetNode<Sprite2D>("Sprite");
		this.stump = this.GetNode<Sprite2D>("Stump");
		this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
		this.applePositions = this.GetNode<Node2D>("ApplePositions");
		this.appleMarker = new List<Marker2D>();

		foreach (var child in this.applePositions.GetChildren())
		{
			if (child is Marker2D marker)
			{
				this.appleMarker.Add(marker);
			}
		}

		int[] frames = { 0, 1 };
		this.treeSprite.Frame = frames[(int)(GD.Randi() % (uint)frames.Length)];
		this.apple = this.GetNode<Node2D>("Apple");

		//Because "this.treeSprite.Material.ResourceLocalToScene = true;" not working
		if (this.treeSprite.Material != null)
		{
			this.treeSprite.Material = (Material)this.treeSprite.Material.Duplicate();
		}

		this.CreateApples(GD.RandRange(0,3));
		this.stump.Hide();
	}

	public void Flash(int damage)
	{
		Tween tween = this.GetTree().CreateTween();
		tween.TweenProperty(this.treeSprite.Material, "shader_parameter/progress", 1.0, 0.2);
		tween.TweenProperty(this.treeSprite.Material, "shader_parameter/progress", 0.0, 0.4);
		this.health -= damage;
		if (this.health <= 0)
		{
			this.stump.Show();
			this.treeSprite.Hide();
			this.collisionShape.Position = new Vector2(0, 10);
			this.collisionShape.Scale = new Vector2(1, 0.4f);
			this.apple.Hide();
			if (this.apple.GetChildCount() >= 0)
			{
				foreach (var apple in this.apple.GetChildren())
				{
					apple.QueueFree();
					
				}
			}
		}
	}

	public void GetApple()
	{
		if (this.apple.GetChildCount() > 0)
		{
			this.apple.GetChildren().PickRandom().QueueFree();
		}
	}

	private void CreateApples(int value)
	{
		for (int i = 0; i < value; i++)
		{
			Marker2D marker = this.PopRandomMarker();
			if (marker == null)
			{
				break;
			}

			Sprite2D appleSprite = new();
			appleSprite.Texture = TextureCache.Apple;

			this.apple.CallDeferred("add_child", appleSprite);

			appleSprite.Position = marker.Position;
		}
	}

	private Marker2D PopRandomMarker()
	{
		if (this.appleMarker.Count == 0)
		{
			return null;
		}

		int index = (int)(GD.Randi() % this.appleMarker.Count);
		Marker2D marker = this.appleMarker[index];
		this.appleMarker.RemoveAt(index);
		return marker;
	}

	public void Reset()
	{
		this.health = 5;
		this.stump.Hide();
		this.treeSprite.Show();
		this.collisionShape.Position = new Vector2(0, 0);
		this.collisionShape.Scale = new Vector2(1, 1);
		this.CreateApples(GD.RandRange(0, 3));
		this.apple.Show();
		if (this.apple.GetChildCount() >= 0)
		{
			foreach (var apple in this.apple.GetChildren())
			{
				apple.QueueFree();

			}
		}
	}
}