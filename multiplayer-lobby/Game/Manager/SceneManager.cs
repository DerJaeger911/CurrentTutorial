using Godot;
using System;

public partial class SceneManager : Node2D
{
	private PackedScene menuScene = GD.Load<PackedScene>("res://Game/Menu.tscn");
	private PackedScene gameScene = GD.Load<PackedScene>("res://Game/Game.tscn");

	private Node game;
	private MenuManager menu;


	public override void _Ready()
	{
		this.game = this.gameScene.Instantiate();
		this.menu = (MenuManager)this.menuScene.Instantiate();
		this.AddChild(this.menu);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void LoadMenuScene()
	{
		if (this.game.GetParent() == this)
		{
			this.RemoveChild(this.game);
		}
		if (this.menu.GetParent() == null)
		{
			this.AddChild(this.menu);
		}
			
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void LoadGameScene()
	{
		if (this.menu.GetParent() == this)
		{
			this.RemoveChild(this.menu);
		}
		if (this.game.GetParent() == null)
		{
			this.AddChild(this.game);
		}
	}
}
