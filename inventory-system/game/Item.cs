using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
	[Export]
	public string DisplayName {get; set;}
	[Export]
	public Texture2D Icon {get; set;}
	[Export]
	public int MaxStackSize {get; set;} = 12;
	[Export]
	public PackedScene WorldItemScene {get; set;}

	public bool OnUse (Player player)
	{
		GD.Print("Use");
		return player.UseItems;
	}
}
