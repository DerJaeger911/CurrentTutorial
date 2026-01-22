using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.AutoLoads.Managers;
using Dcozysandbox.Scripts.Enums;
using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class ResourceUi : Control
{
    private HBoxContainer container;
    private Timer timer;
    private Label appleLabel;
    private Label woodLabel;
    private Label fishLabel;
    private Label cornLabel;
    private Label tomatoLabel;
    private Label pumpkinLabel;

    public override void _Ready()
    {
		this.container = this.GetNode<HBoxContainer>("HBoxContainer");
        this.timer = this.GetNode<Timer>("Timer");
		this.appleLabel = this.GetNode<Label>("HBoxContainer/Apple/Label");
		this.woodLabel = this.GetNode<Label>("HBoxContainer/Wood/Label");
		this.fishLabel = this.GetNode<Label>("HBoxContainer/Fish/Label");
		this.cornLabel = this.GetNode<Label>("HBoxContainer/Corn/Label");
		this.tomatoLabel = this.GetNode<Label>("HBoxContainer/Tomato/Label");
		this.pumpkinLabel = this.GetNode<Label>("HBoxContainer/Pumpkin/Label");

        this.container.AnchorTop = 1.15f;
		this.container.AnchorBottom = 1.15f;

		this.LoadLabeltexts();
        SignalBus.Instance.ResourceCountChanged += this.LoadLabeltexts;
        this.timer.Timeout += this.OnTimeTimeOut;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("show_resources"))
        {
            this.ShowResources();
        }
    }

    private void LoadLabeltexts()
    {
        this.appleLabel.Text = PlayerResourceManager.Instance.GetResourceCount(ResourceEnum.Apple).ToString();
		this.woodLabel.Text = PlayerResourceManager.Instance.GetResourceCount(ResourceEnum.Wood).ToString();
        this.fishLabel.Text = PlayerResourceManager.Instance.GetResourceCount(ResourceEnum.Fish).ToString();
		this.cornLabel.Text = PlayerResourceManager.Instance.GetResourceCount(ResourceEnum.Corn).ToString();
		this.tomatoLabel.Text = PlayerResourceManager.Instance.GetResourceCount(ResourceEnum.Tomato).ToString();
		this.pumpkinLabel.Text = PlayerResourceManager.Instance.GetResourceCount(ResourceEnum.Pumpkin).ToString();
        this.ShowResources();
	}

    public void ShowResources()
    {
		this.TweenAnimation(1);
		this.timer.Start();
	}

    private void OnTimeTimeOut()
    {
        this.TweenAnimation(1.15f);
	}

    private void TweenAnimation(float target)
    {
		Tween tween = this.GetTree().CreateTween();
		tween.SetParallel(true);
		tween.TweenProperty(this.container, "anchor_top", target, 0.5);
		tween.TweenProperty(this.container, "anchor_bottom", target, 0.5);
	}

    public override void _ExitTree()
    {
		SignalBus.Instance.ResourceCountChanged -= this.LoadLabeltexts;
		this.timer.Timeout -= this.OnTimeTimeOut;
	}
}
