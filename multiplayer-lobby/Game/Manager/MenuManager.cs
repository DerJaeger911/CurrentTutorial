using Godot;
using MultiplayerLobby.Game.Manager;
using System;

public partial class MenuManager : Control
{
	private NetworkManager network;
	private Control mainScreen;
	private Control lobbyScreen;

	public override void _Ready()
	{
		this.network = this.GetNode<NetworkManager>("/root/Main/NetworkManager");
		this.mainScreen = this.GetNode<Control>("CanvasLayer/BG/MainMenu");
		this.lobbyScreen = this.GetNode<Control>("CanvasLayer/BG/Lobby");

		this.network.ConnectedToServer += this.OnOpenLobby;
		this.network.ServerDisconnected += this.OnOpenMainMenu;
	}

	public void OnOpenMainMenu()
	{
		this.mainScreen.Visible = true;
		this.lobbyScreen.Visible = false;
	}

	private void OnOpenLobby() 
	{
		this.mainScreen.Visible = false;
		this.lobbyScreen.Visible = true;
	}
}
