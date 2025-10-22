using Godot;
using GodotRTSCourse.Scripts.Game;

public partial class PlayerUnit : Node
{
    private Sprite2D selectionVisual;

    public override void _Ready()
    {
        selectionVisual = GetNode<Sprite2D>(GodotConsts.SelectionVisual);
    }

    public void ToggleSelectionVisual(bool toggle)
    {
        selectionVisual.Visible = toggle;
    }
}
