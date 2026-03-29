using Godot;
using MultiplayerLobby.Game.Manager;
using System;
using System.Collections.Generic;

namespace MultiplayerLobby.Game;

public partial class Game : Node2D
{
	private NetworkManager network;
	private PackedScene playerCharScene = GD.Load<PackedScene>("res://Game/player_character.tscn");
	private MultiplayerSpawner spawner;

	private int playersInGame;
	private List<Node> currentCharacters = new();

	public override void _Ready()
	{
		this.spawner = this.GetNode<MultiplayerSpawner>("MultiplayerSpawner");
		this.network = this.GetNode<NetworkManager>("/root/Main/NetworkManager");

		this.Rpc(nameof(ImInGame), this.Multiplayer.GetUniqueId());
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void ImInGame(int id)
	{
		if (this.Multiplayer.IsServer())
		{
			this.playersInGame++;

			if(this.playersInGame == this.network.CurrentPlayers.Count)
			{
				this.SpawnPlayer();
			}
		}
	}

	private void SpawnPlayer()
	{
		foreach (Node player in this.network.CurrentPlayers)
		{
			if (player is Player spawnablePlayer)
			{
				this.SpawnPlayerCharacter(spawnablePlayer);
			}
		}
	}

	private void SpawnPlayerCharacter(Player player)
	{
		PlayerCharacter character = this.playerCharScene.Instantiate<PlayerCharacter>();
		character.Name = player.Name;
		this.spawner.AddChild(character, true);
		character.GlobalPosition = new Vector2(100, -200);
		this.currentCharacters.Add(character);
	}
}
