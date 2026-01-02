using Godot;
using System;
using System.Collections.Generic;

namespace bullethellcourse.Scripts.Bullets;

public abstract partial class BulletPool : Node
{
	private PackedScene nodeScene = GD.Load<PackedScene>("res://Scenes/bullet.tscn");
	private List<Node2D> cachedNodes = new();

	private Node2D CreateNew()
	{
		Node2D node = this.nodeScene.Instantiate<Node2D>();
		this.cachedNodes.Add(node);
		this.GetTree().GetRoot().CallDeferred(Node.MethodName.AddChild, node);

		return node;
	}

	public Node2D Spawn()
	{
		foreach (Node2D node in this.cachedNodes)
		{
			if (!node.Visible)
			{
				node.Visible = true;
				return node;
			}
		}
		return this.CreateNew();
	}
}