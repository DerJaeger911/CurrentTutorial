using Godot;
using System.Collections.Generic;

namespace bullethellcourse.Scripts.Pools;

public abstract partial class PoolBase : Node
{
	protected abstract PackedScene nodeScene { get; }
	private List<Node2D> cachedNodes = new();

	private Node2D CreateNew(Node parent)
	{
		Node2D node = this.nodeScene.Instantiate<Node2D>();
		this.cachedNodes.Add(node);
		parent.CallDeferred(Node.MethodName.AddChild, node);

		return node;
	}

	public Node2D Spawn(Node parent)
	{
		foreach (Node2D node in this.cachedNodes)
		{
			if (!node.Visible)
			{
				node.Visible = true;
				parent.CallDeferred(Node.MethodName.AddChild, node);
				return node;
			}
		}

		return this.CreateNew(parent);
	}
}
