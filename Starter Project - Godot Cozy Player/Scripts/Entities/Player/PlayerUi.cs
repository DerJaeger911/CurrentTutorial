using Dcozysandbox.Scripts.AutoLoads.Busses;
using Godot;

public partial class PlayerUi : Node2D
{
	private Sprite2D main;
	private Sprite2D leftSprite;
	private Sprite2D rightSprite;
	private Node2D tools;
	private Timer timer;

    public override void _Ready()
    {
		this.timer = this.GetNode<Timer>("Timer");
		this.tools = this.GetNode<Node2D>("Tools");
        this.main = this.GetNode<Sprite2D>("Tools/Main");
		this.leftSprite = this.GetNode<Sprite2D>("Tools/LeftSprite");
		this.rightSprite = this.GetNode<Sprite2D>("Tools/RightSprite");
		SignalBus.Instance.ToolChanged += this.OnToolChanged;
		this.tools.Hide();
		this.timer.Timeout += this.OnTimerTimeOut;
	}

	private void Reveal()
	{
		this.tools.Show();
		this.timer.Start();
	}

	private void OnToolChanged(int tool)
	{
		this.main.Frame = tool;
		this.leftSprite.Frame = Mathf.PosMod(tool - 1, this.leftSprite.Hframes);
		this.rightSprite.Frame = Mathf.PosMod(tool + 1, this.rightSprite.Hframes);
		this.Reveal();
		this.ZIndex = 2;
	}

	private void OnTimerTimeOut()
	{
		this.tools.Hide();
	}


	public override void _ExitTree()
    {
		SignalBus.Instance.ToolChanged -= this.OnToolChanged;
	}
}
