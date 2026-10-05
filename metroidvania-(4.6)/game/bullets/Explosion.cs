using Godot;
using Metroidvania.game.interfaces;
using System;
using System.Collections.Generic;

public partial class Explosion : Area2D
{
	private List<Node2D> targets = new List<Node2D>();
	private Node2D parent;

	override public void _Ready()
	{
		this.BodyEntered += this.OnBodyEntered;
	}

	public void Setup(Node2D parent)
	{
		this.parent = parent;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Node2D target && !this.targets.Contains(target) && target != this.parent)
		{
			this.targets.Add(target);
			GD.Print($"Explosion hit: {target.Name}");
			if (target is IDamageable damageable)
			{
				damageable.Hit();
			}
		}
	}
}
