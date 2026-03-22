using Godot;
using MultiplayerLobby.Game.Manager;
using System;

namespace MultiplayerLobby.Game;

public partial class PlayerSlot : Panel
{
	private Label nameText;
	private Button kickButton;
	private Player currentPlayer;

	public override void _Ready()
	{
		this.nameText = this.GetNode<Label>("MarginContainer/HBoxContainer/PlayerName");
		this.kickButton = this.GetNode<Button>("MarginContainer/HBoxContainer/AspectRatioContainer/MarginContainer/KickButton");

		this.kickButton.Pressed += this.OnKickButtonPressed;
	}

	public void UpdateUiSlot(Player player)
	{
		this.currentPlayer = player;
		this.nameText.Text = player.Username;

		bool isLocal = player.IsMultiplayerAuthority();

		if (this.Multiplayer.IsServer())
		{
			this.kickButton.Visible = !isLocal;
		}
		else
		{
			this.kickButton.Visible = false;
		}
	}

	private void OnKickButtonPressed()
	{
		if (!this.Multiplayer.IsServer())
		{
			return;
		}

		var networkManager = this.GetNode<NetworkManager>("/root/Main/NetworkManager");
		networkManager.Rpc(nameof(networkManager.DisconnectPlayer), this.currentPlayer.PlayerId);
	}

	public override void _ExitTree()
	{
		this.kickButton.Pressed -= this.OnKickButtonPressed;
	}
}
