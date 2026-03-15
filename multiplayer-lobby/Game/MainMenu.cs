using Godot;
using MultiplayerLobby.Game.Manager;
using System;

public partial class MainMenu : Control
{
	private NetworkManager networkManager;
	private LineEdit usernameInput;
	private LineEdit ipInput;
	private LineEdit portInput;
	[Export]
	private Button createLobbyButton;
	[Export]
	private Button joinLobbyButton;


	public override void _Ready()
	{
		this.networkManager = this.GetNode<NetworkManager>("/root/Main/NetworkManager");
		this.usernameInput = this.GetNode<LineEdit>("VBoxContainer/UserNameInput");
		this.ipInput = this.GetNode<LineEdit>("VBoxContainer/IpInput");
		this.portInput = this.GetNode<LineEdit>("VBoxContainer/PortInput");

		this.createLobbyButton.Pressed += this.OnCreateLobbyButtonPressed;
		this.joinLobbyButton.Pressed += this.OnJoinLobbyButtonPressed;
	}

	private void OnCreateLobbyButtonPressed()
	{
		this.networkManager.LocalUsername = this.usernameInput.Text;
		if (int.TryParse(this.portInput.Text, out int port))
		{
			this.networkManager.StartHost(port);
		}
		else
		{
			GD.PrintErr("Ungültiger Port! Bitte nur Zahlen eingeben.");
		}
	}

	private void OnJoinLobbyButtonPressed()
	{

	}
}
