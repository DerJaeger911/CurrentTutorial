using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
	[Export]
	private string displayName;
	[Export]
	public Texture2D Icon {get; set;}
	[Export]
	public int MaxStackSize {get; set;} = 12;
	[Export]
	private PackedScene worldItemScene;

	private bool OnUse (Player player)
	{
		return false;
	}
}
