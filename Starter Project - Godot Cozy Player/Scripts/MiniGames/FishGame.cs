using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class FishGame : Control
{
    private TextureRect fishRect;
    public override void _Ready()
    {
        this.fishRect = this.GetNode<TextureRect>("FishRect");
        var tween = this.GetTree().CreateTween();
        tween.SetLoops();
        tween.TweenProperty(this.fishRect, "offset_left", 100, 1);
		tween.TweenProperty(this.fishRect, "offset_left", -60, 1);

	}

    public bool GetFish()
    {
        if(MathF.Abs(this.fishRect.OffsetLeft) < 40)
        {
            return true;
        }
        return false;
    }
}
