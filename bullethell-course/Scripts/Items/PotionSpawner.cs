using bullethellcourse.Scripts.Items;
using bullethellcourse.Scripts.Managers;
using Godot;
using System;
using System.Collections.Generic;

public partial class PotionSpawner : Node
{
	private int itemCount;
	private Vector2 minBounds = new Vector2(-440, -440);
	private Vector2 maxBounds = new Vector2(440, 440);

	private Timer spawnTimer;

    public override void _Ready()
    {
		this.spawnTimer = this.GetNode<Timer>("SpawnTimer");
		this.spawnTimer.Timeout += this.OnSpawnTimerTimeout;
		this.itemCount = ItemLoadManager.Instance.GetItemCount();
	}

	private void OnSpawnTimerTimeout()
	{
		PotionTypeEnum potionType = (PotionTypeEnum)(GD.Randi() % this.itemCount);
		GD.Print(potionType);
		Potion potion = ItemLoadManager.Instance.GetItem(potionType).Instantiate() as Potion;
		this.CallDeferred(Node.MethodName.AddChild, potion);

		float spawnX = (float)GD.RandRange(this.minBounds.X, this.maxBounds.X);
		float spawnY = (float)GD.RandRange(this.minBounds.Y, this.maxBounds.Y);
		potion.GlobalPosition = new Vector2(spawnX, spawnY);
		GD.Print(potion.GlobalPosition);
	}
}
