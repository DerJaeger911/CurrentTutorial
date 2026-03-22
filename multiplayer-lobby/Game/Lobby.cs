using Godot;
using MultiplayerLobby.Game;
using MultiplayerLobby.Game.Manager;
using System;
using System.Collections.Generic;

public partial class Lobby : Control
{
	private NetworkManager network;
	private SceneManager sceneManager;

	private List<PlayerSlot> playerSlots = new();
	private VBoxContainer playerSlotList;
	private Button startButton;
	private Button leaveLobbyButton;
	private Timer lobbyUpdateTimer;

	public override void _Ready()
	{
		this.startButton = this.GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer2/StartGame");
		this.leaveLobbyButton = this.GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer2/LeaveLobby");
		this.network = this.GetNode<NetworkManager>("/root/Main/NetworkManager");
		this.playerSlotList = this.GetNode<VBoxContainer>("MarginContainer/HBoxContainer/ScrollContainer/PlayerSlotList");
		this.lobbyUpdateTimer = this.GetNode<Timer>("LobbyUpdateTimer");
		this.sceneManager = this.GetNode<SceneManager>("/root/Main");

		this.startButton.Pressed += this.OnStartButtonPressed;
		this.leaveLobbyButton.Pressed += this.OnLeaveLobbyButtonPressed;
		this.lobbyUpdateTimer.Timeout += this.UpdateUi;

		foreach (PlayerSlot slot in this.playerSlotList.GetChildren())
		{
			this.playerSlots.Add(slot);
		}
	}

	private void UpdateUi()
	{
		if (!this.Visible)
		{
			return;
		}

		this.startButton.Visible = this.Multiplayer.IsServer();

		List<Node> players = this.network.CurrentPlayers;

		for(int i=0; i < this.playerSlots.Count; i++)
		{
			var slot = this.playerSlots[i];

			slot.Visible = i < players.Count;

			if (slot.Visible)
			{
				slot.UpdateUiSlot((Player)players[i]);
			}
		}
	}

	private void OnStartButtonPressed()
	{
		this.sceneManager.Rpc(nameof(this.sceneManager.LoadGameScene));
	}

	private void OnLeaveLobbyButtonPressed()
	{
		if (this.Multiplayer.IsServer())
		{
			this.GetNode<MenuManager>("../../..").OnOpenMainMenu();
		}
		this.Multiplayer.MultiplayerPeer.Close();
	}

	public override void _ExitTree()
	{
		this.startButton.Pressed -= this.OnStartButtonPressed;
		this.lobbyUpdateTimer.Timeout -= this.UpdateUi;
	}
}
