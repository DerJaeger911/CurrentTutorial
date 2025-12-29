using Godot;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Items;
using RoguelikeCourse.Scripts.Statics;

public partial class Item : Area2D
{

	//Item on right layer!!! Now it is wrong. Oh and both ways!!!
	[Export]
	private StatEnum itemType;

	[Export]
	private float itemValue;

    public override void _Ready()
    {
		this.CollisionLayer = LayerMasks.BaseLayer;
		this.CollisionMask = LayerMasks.PlayerLayer;
        BodyEntered += this.OnBodyEntered;
    }

	private void OnBodyEntered(Node body)
	{
        if (body is null || body is not Player player)
		{
			return;
		}

		
		switch (this.itemType)
		{
			case StatEnum.Health:
				player.Heal((int)this.itemValue);
				break;
			case StatEnum.ShootRate:
				if(player.ShootRate - this.itemValue <0.1)
				{
                    player.ShootRate = 0.1;
				}
				else
				{
                    player.ShootRate -= this.itemValue;
				}
				break;
			case StatEnum.MoveSpeed:
                player.MoveSpeed += this.itemValue;
				break;
		}
		player.ItemCollectedSound.Play();

		PrintStats.AllStats(player, this.itemType);
		this.QueueFree();
	}

	private string GetDebuggerDisplay()
	{
		return this.ToString();
	}

    public override void _ExitTree()
    {
        BodyEntered -= this.OnBodyEntered;
    }
}
