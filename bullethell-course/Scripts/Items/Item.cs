using bullethellcourse.Scripts.Entities;
using bullethellcourse.Scripts.Statics;
using Godot;

namespace bullethellcourse.Scripts.Items;

public abstract partial class Item : Area2D
{
	public override void _Ready()
	{
		this.CollisionLayer = LayerMask.ItemLayer;
		this.CollisionMask = LayerMask.ItemMask;
		this.BodyEntered += this.OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if(body is Player player)
		{
			this.ProcessItemEffects(player);
		}
		else
		{
			return;
		}

		this.QueueFree();
	}

	protected abstract void ProcessItemEffects(Player player);

    public override void _ExitTree()
    {
		this.BodyEntered -= this.OnBodyEntered;
	}
}
