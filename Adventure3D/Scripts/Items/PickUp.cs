using Adventuregame.Scripts.Characters.Player;
using Adventuregame.Scripts.GlobalData;
using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

public partial class PickUp : Node3D
{
	[Export]
	private AllItemEnum itemType;
	[Export] 
	private float floatSpeed = 0.005f;
	[Export] 
	private float floatAmplitude = 0.5f;

	private Node3D itemNode;
	private Area3D area;
	private Enum specificItem;

	public override void _Ready()
	{
		this.AddItemNode();
		this.area = this.GetNode<Area3D>("Area3D");
		this.area.BodyEntered += this.OnBodyEntered;
	}

	public override void _PhysicsProcess(Double delta)
	{
		this.itemNode.RotateY((float)(2 * delta));

		float positiveSin = (Mathf.Sin(Time.GetTicksMsec() * this.floatSpeed) + 1) / 2;
		float boppY = positiveSin * this.floatAmplitude;
		this.itemNode.Position = new Vector3(this.itemNode.Position.X, boppY, this.itemNode.Position.Z);
	}

	private async void OnBodyEntered(Node3D body)
	{
		if (body is Player player)
		{
			if(this.specificItem is WeaponEnum)
			{
				if (!player.CollectedWeapons.Contains(Equipment.Instance.Weapons[(WeaponEnum)this.specificItem]))
				{
					player.CollectedWeapons.Add(Equipment.Instance.Weapons[(WeaponEnum)this.specificItem]);
				}
			}
			else if (this.specificItem is ShieldEnum)
			{
				if (!player.CollectedShields.Contains(Equipment.Instance.Shields[(ShieldEnum)this.specificItem]))
				{
					player.CollectedShields.Add(Equipment.Instance.Shields[(ShieldEnum)this.specificItem]);
				}
			}
			else if (this.specificItem is StyleEnum)
			{
				if (!player.CollectedStyles.Contains(Equipment.Instance.Styles[(StyleEnum)this.specificItem]))
				{
					player.CollectedStyles.Contains(Equipment.Instance.Styles[(StyleEnum)this.specificItem]);
				}
			}
			this.area.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true; 
			Tween tween = this.CreateTween();
			tween.TweenProperty(this, "scale", new Vector3(0.1f, 0.1f, 0.1f), 0.5);
			tween.Finished += () => this.QueueFree();
		}
	}

	private void AddItemNode()
	{
		this.specificItem = ItemEnumConverter.ConvertItemEnum(this.itemType);

		var allCategories = new System.Collections.IEnumerable[]
		{
			Equipment.Instance.Weapons,
			Equipment.Instance.Shields,
			Equipment.Instance.Styles
		};
		foreach (var category in allCategories)
		{
			var dict = (System.Collections.IDictionary)category;
			if (dict.Contains(this.specificItem))
			{
				dynamic data = dict[this.specificItem];
				this.itemNode = (Node3D)data.Scene.Instantiate();
				this.AddChild(this.itemNode);
				return;
			}
		}
	}
}
