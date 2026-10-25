using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
	[Export]
	private string displayName;
	[Export]
	private Texture2D Icon;
	[Export]
	private int maxStackSize = 12;
	[Export]
	private PackedScene worldItemScene;

	private bool OnUse (Player player)
	{
		return false;
	}
}
