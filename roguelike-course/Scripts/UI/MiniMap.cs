using Godot;
using Godot.Collections;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Manager.Signals;

public partial class MiniMap : GridContainer
{
	private RoomGeneration roomGeneration;

	private Array<TextureRect> icons = [];

	private Texture2D roomTexture;

	private Texture2D playerRoomTexture;

	public override void _Ready()
	{
		this.roomTexture = GD.Load<Texture2D>("res://Assets/Sprites/Items/minimap_room.tres");
		this.playerRoomTexture = GD.Load<Texture2D>("res://Assets/Sprites/Items/minimap_player_room.tres");
		GameSignals.Instance.PlayerEnteredRoom += this.OnPlayerEnteredRoom;
	}

	private void OnPlayerEnteredRoom(Room room)
	{
		if (this.roomGeneration == null)
		{
			this.roomGeneration = this.GetNode<RoomGeneration>("/root/Main/RoomGeneration");
			if (this.roomGeneration == null)
			{
				return;
			}

			foreach (Node child in this.GetChildren())
			{
				if (child is TextureRect tr)
				{
					this.icons.Add(tr);
				}
			}
		}

		foreach (int x in GD.Range(this.roomGeneration.MapSize))
		{
			foreach (int y in GD.Range(this.roomGeneration.MapSize))
			{
				Room r = this.roomGeneration.GetRoomFromMap(x, y);
				int i = x + y * this.roomGeneration.MapSize;

                if (i >= this.icons.Count)
                {
					continue;
                }

				if (r == null)
				{
					this.icons[i].Texture = null;
				}
				else if (r == room)
				{
					this.icons[i].Texture = this.playerRoomTexture;
				}
				else
				{
					this.icons[i].Texture = this.roomTexture;
				}
            }
		}
	}

	public override void _ExitTree()
	{
		GameSignals.Instance.PlayerEnteredRoom -= this.OnPlayerEnteredRoom;
	}
}
