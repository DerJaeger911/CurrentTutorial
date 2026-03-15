using Godot;
using System;
using System.Collections.Generic;


namespace MultiplayerLobby.Game.Manager;
public partial class NetworkManager : Node
{
	private PackedScene playerScene;
	private MultiplayerSpawner spawnedNodes;
	private List<Node> currentPlayer = new();

	public String LocalUsername { get; set; }

	[Signal]
	public delegate void ConnectedToServerEventHandler();

	[Signal]
	public delegate void ServerDisconnectedEventHandler();

	public override void _Ready()
	{
		this.playerScene = GD.Load<PackedScene>("res://Game/Player.tscn");
		this.spawnedNodes = this.GetNode<MultiplayerSpawner>("MultiplayerSpawner");

		this.Multiplayer.PeerConnected += this.OnPlayerConnected;
		this.Multiplayer.PeerDisconnected += this.OnPlayerDisconnected;

		this.Multiplayer.ConnectedToServer += this.OnConnectedToServer;
		this.Multiplayer.ConnectionFailed += this.OnConnectionFailed;
		this.Multiplayer.ServerDisconnected += this.OnServerDisconnected;
	}

	public void StartHost(int port)
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(port, 8);
		this.Multiplayer.MultiplayerPeer = peer;

		this.OnPlayerConnected(this.Multiplayer.GetUniqueId());
		this.OnConnectedToServer();
	}

	public void StartClient(string ip, int port)
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(ip, port);
		this.Multiplayer.MultiplayerPeer = peer;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void DisconnectPlayer(long playerId)
	{
		if (this.Multiplayer.MultiplayerPeer is ENetMultiplayerPeer enetPeer)
		{
			enetPeer.DisconnectPeer((int)playerId);
		}
	}

	private void OnPlayerConnected(long playerId)
	{
		if (!this.Multiplayer.IsServer())
		{
			return;
		}

		GD.Print("New Player has joined!");
		var player = this.playerScene.Instantiate();
		player.Name = playerId.ToString();
		this.spawnedNodes.AddChild(player, true);
	}

	private void OnPlayerDisconnected(long playerId) 
	{
		var node = this.spawnedNodes.GetNode(playerId.ToString());

		if (this.currentPlayer.Contains(node))
		{
			this.RemovePlayerFormList(node);
		}

		if (node != null)
		{
			node.QueueFree();
		}
	}

	private void OnConnectedToServer()
	{
		this.EmitSignal(SignalName.ConnectedToServer);
	}

	private void OnConnectionFailed()
	{
		this.EmitSignal(SignalName.ServerDisconnected);
	}

	private void OnServerDisconnected() 
	{

	}

	private void AddPlayerFormList(Node player)
	{
		this.currentPlayer.Add(player);
	}

	private void RemovePlayerFormList(Node player)
	{
		this.currentPlayer.Remove(player);
	}

	public override void _ExitTree()
	{
		this.Multiplayer.PeerConnected -= this.OnPlayerConnected;
		this.Multiplayer.PeerDisconnected -= this.OnPlayerDisconnected;

		this.Multiplayer.ConnectedToServer -= this.OnConnectedToServer;
		this.Multiplayer.ConnectionFailed -= this.OnConnectionFailed;
		this.Multiplayer.ServerDisconnected -= this.OnServerDisconnected;
	}
}
