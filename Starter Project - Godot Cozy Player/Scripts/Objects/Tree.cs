using Dcozysandbox.Scripts.AutoLoads.Managers;
using Dcozysandbox.Scripts.Entities.Player;
using Dcozysandbox.Scripts.Enums;
using Dcozysandbox.Scripts.Helper;
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
	private bool isAlive;
	private Player player;

	public override void _Ready()
	{
		this.treeSprite = this.GetNode<Sprite2D>("Sprite");
		this.stump = this.GetNode<Sprite2D>("Stump");
		this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
		this.applePositions = this.GetNode<Node2D>("ApplePositions");
		this.appleMarker = new List<Marker2D>();
		this.apple = this.GetNode<Node2D>("Apple");
		this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");

		this.isAlive = GD.Randf() > 0.5f;
		this.SetTreeState(this.isAlive);

		this.appleMarker = this.applePositions.GetChildren().OfType<Marker2D>().ToList();

		int[] frames = { 0, 1 };
		this.treeSprite.Frame = frames[(int)(GD.Randi() % (uint)frames.Length)];
		
		//Because "this.treeSprite.Material.ResourceLocalToScene = true;" not working
		if (this.treeSprite.Material != null)
		{
			this.treeSprite.Material = (Material)this.treeSprite.Material.Duplicate();
		}

		this.CreateApples(GD.RandRange(0,3));
	}

	public void TakeDamage(int damage)
	{
		this.health -= damage;

		ShaderHelper.Flash(this, this.treeSprite.Material);
		this.GetApple();

		if (this.health <= 0)
		{
			this.Die();
			PlayerResourceManager.Instance.AddResource(ResourceEnum.Wood, 1);
		}
	}

	private void Die() => this.SetTreeState(false);

	private void SetTreeState(bool isAlive)
	{
		this.stump.Visible = !isAlive;
		this.treeSprite.Visible = isAlive;
		this.apple.Visible = isAlive;

		this.collisionShape.Position = isAlive ? Vector2.Zero : new Vector2(0, 10);
		this.collisionShape.Scale = isAlive ? Vector2.One : new Vector2(1, 0.4f);

		this.FreeApples();
	}

	public void Reset()
	{
		this.health = 5;
		this.SetTreeState(true);
		this.CreateApples(GD.RandRange(0, 3));
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

	public void GetApple()
	{
		if (this.apple.GetChildCount() > 0)
		{
			PlayerResourceManager.Instance.AddResource(ResourceEnum.Apple, 1);
			PlayerResourceManager.Instance.PrintAll();
			this.apple.GetChildren().PickRandom().QueueFree();
		}
	}

	private void FreeApples()
	{
		if (this.apple.GetChildCount() >= 0)
		{
			foreach (var apple in this.apple.GetChildren())
			{
				apple.QueueFree();
			}
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
}