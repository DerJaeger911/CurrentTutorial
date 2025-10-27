using Godot;
using GodotRTSCourse.Scripts.Game;
using GodotRTSCourse.Scripts.Game.Interfaces;

namespace GodotRTSCourse.Scripts.Game;

public partial class PlayerUnit : Node, IUnitPlayer
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
