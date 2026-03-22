using Godot;
using MultiplayerLobby.Game.Manager;
using System;

namespace MultiplayerLobby.Game;

public partial class Player : Node
{
	private NetworkManager network;
	[Export]
	private string username;
	[Export]
	private int playerId;

	public String Username { get => this.username; set => this.username = value; }
	public Int32 PlayerId { get => this.playerId; set => this.playerId = value; }

	public override void _EnterTree()
	{
		if (int.TryParse(this.Name, out int id))
		{
			this.PlayerId = id;
			this.SetMultiplayerAuthority(id);
		}
	}

	public override void _Ready()
	{
		this.network = this.GetNode<NetworkManager>("/root/Main/NetworkManager");
		this.Username = this.network.LocalUsername;
		this.network.AddPlayerToList(this);
	}

	public override void _Notification(Int32 what)
	{
		if (what == NotificationPredelete)
		{
			this.network.RemovePlayerFormList(this);
		}
	}
} 