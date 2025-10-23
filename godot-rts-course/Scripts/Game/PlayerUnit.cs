using Godot;
using GodotRTSCourse.Scripts.Game;

namespace GodotRTSCourse.Scripts.Game;

public partial class PlayerUnit : Node
{
    private Sprite2D selectionVisual;

    public override void _Ready()
    {
		this.selectionVisual = GetNode<Sprite2D>(GodotConsts.SelectionVisual);
    }

    public void ToggleSelectionVisual(bool toggle)
    {
		this.selectionVisual.Visible = toggle;
    }
}
