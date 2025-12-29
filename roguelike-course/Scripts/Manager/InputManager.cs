using Godot;
using RoguelikeCourse.Scripts.Manager.PreloadManagers.AssetPreloads;
using RoguelikeCourse.Scripts.Manager.Signals;

namespace RoguelikeCourse.Scripts.Manager;

public partial class InputManager : Node
{
	private bool showCrosshair;
	private Node2D crosshair;

    public override void _Ready()
    {
		GameSignals.Instance.InMenuState += this.ShowCrosshair;

        var layer = new CanvasLayer();
        this.CallDeferred("add_child", layer);

        this.crosshair = AssetPreloads.Crosshair;
        this.crosshair.ZIndex = 1000;
        layer.CallDeferred("add_child", this.crosshair);
    }

    public override void _Process(double delta)
    {
        if (!this.showCrosshair)
		{
			return;
		}

        this.crosshair.Position = this.GetViewport().GetMousePosition();
    }

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}

	private void ShowCrosshair(bool isInMenu)
	{
       this.showCrosshair = !isInMenu;
       Input.MouseMode = this.showCrosshair ? Input.MouseModeEnum.Hidden : Input.MouseModeEnum.Visible;
       this.crosshair.Visible = this.showCrosshair ? true : false;
        GD.Print("worked");
    }

    public override void _ExitTree()
    {
        GameSignals.Instance.InMenuState -= this.ShowCrosshair;
    }
}