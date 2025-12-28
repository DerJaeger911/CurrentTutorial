using Godot;
using Godot.Collections;
using RoguelikeCourse.Scripts.Manager.Signals;

namespace RoguelikeCourse.Scripts.UI;

public partial class PlayerHealth : GridContainer
{
	private Array<TextureRect> heartIcons = [];
	private Texture2D fullHeart;
	private Texture2D emptyHeart;

    public override void _Ready()
    {
        this.fullHeart = GD.Load<Texture2D>("res://Assets/Sprites/Items/heart_full.tres");
        this.emptyHeart = GD.Load<Texture2D>("res://Assets/Sprites/Items/heart_empty.tres");

        GameSignals.Instance.PlayerUpdateHealth += this.OnUpdateHealth;

        foreach(var child in this.GetChildren())
        {
            if (child is TextureRect tr)
            {
                this.heartIcons.Add(tr);
            }
        }
    }

    private void OnUpdateHealth(int currentHp, int maxHp)
    {
        for(int i = 0; i < this.heartIcons.Count; i++)
        {
            if(i >= maxHp)
            {
                this.heartIcons[i].Visible = false;
                continue;
            }

            this.heartIcons[i].Visible = true;

            if(i < currentHp)
            {
                this.heartIcons[i].Texture = this.fullHeart;
            }
            else
            {
				this.heartIcons[i].Texture = this.emptyHeart;
			}
        }
    }
    public override void _ExitTree()
    {
		GameSignals.Instance.PlayerUpdateHealth -= this.OnUpdateHealth;
	}
}