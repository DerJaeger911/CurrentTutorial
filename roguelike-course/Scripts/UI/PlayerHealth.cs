using Godot;
using Godot.Collections;

public partial class PlayerHealth : GridContainer
{
	private Array<TextureRect> heartIcons = [];
	private Texture2D fullHeart;
	private Texture2D emptyHeart;

    public override void _Ready()
    {
        this.fullHeart = GD.Load<Texture2D>("res://Assets/Sprites/Items/heart_full.tres");
        this.emptyHeart = GD.Load<Texture2D>("res://Assets/Sprites/Items/heart_empty.tres");

        foreach(var child in this.GetChildren())
        {
            if (child is TextureRect tr)
            {
                this.heartIcons.Add(tr);
            }
        }
    }

}