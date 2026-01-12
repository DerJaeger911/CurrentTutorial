using Godot;
using System;

public partial class Tree : StaticBody2D
{
	private Sprite2D treeSprites;

    public override void _Ready()
    {
        this.treeSprites = this.GetNode<Sprite2D>("Sprite");
		var frames = new int[] { 0, 1 };
		this.treeSprites.Frame = frames[(int)(GD.Randi() % (uint)frames.Length)];
	}
}
