using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Enums;
using Godot;

namespace Dcozysandbox.Scripts.Entities.Player;

public partial class PlayerUi : Node2D
{
	private Sprite2D main;
	private Sprite2D leftSprite;
	private Sprite2D rightSprite;
	private Player player;
	private Node2D tools;
	private Sprite2D seeds;
	private Timer timer;
	private Label stunLabel;

    public override void _Ready()
    {
		this.timer = this.GetNode<Timer>("Timer");
		this.tools = this.GetNode<Node2D>("Tools");
        this.main = this.GetNode<Sprite2D>("Tools/Main");
		this.player = this.GetParent<Player>();
		this.seeds = this.GetNode<Sprite2D>("Seeds");
		this.stunLabel = this.GetNode<Label>("StunLabel");
		this.leftSprite = this.GetNode<Sprite2D>("Tools/LeftSprite");
		this.rightSprite = this.GetNode<Sprite2D>("Tools/RightSprite");
		SignalBus.Instance.ToolChanged += this.OnToolChanged;
		SignalBus.Instance.SeedChanged += this.OnSeedChanged;

		this.tools.Hide();
		this.seeds.Hide();
		this.timer.Timeout += this.OnTimerTimeOut;
		this.stunLabel.Visible = false;
	}

	private void Reveal<T>(T element) where T : CanvasItem
	{
		element.Show();
		this.timer.Start();
	}

	private void OnToolChanged(int tool)
	{
		this.main.Frame = tool;
		this.leftSprite.Frame = Mathf.PosMod(tool - 1, this.leftSprite.Hframes);
		this.rightSprite.Frame = Mathf.PosMod(tool + 1, this.rightSprite.Hframes);
		this.Reveal(this.tools);
		this.ZIndex = 2;
		this.ToggleOtherVisible(true);
	}

	private void OnSeedChanged(int seed)
	{
		SeedEnum currentSeed = (SeedEnum)seed;
		this.seeds.Frame = seed;
		this.Reveal(this.seeds);
		GD.Print(currentSeed);
		this.ToggleOtherVisible(false);
	}

	private void ToggleOtherVisible(bool isTool)
	{
		this.seeds.Visible = !isTool;
		this.tools.Visible = isTool;
	}

	private void OnTimerTimeOut()
	{

		this.seeds.Hide();
		this.tools.Hide();
	}

    public override void _Process(System.Double delta)
    {
        if(this.player.StunTimer == null)
		{
			if (this.stunLabel.Visible)
			{
				this.stunLabel.Visible = false;
			}
			return;
		}
		this.UpdateStunLabel();
    }

	private void UpdateStunLabel()
	{
		this.stunLabel.Visible = true;
		this.stunLabel.Text = $"Stun: {this.player.StunTimer.TimeLeft:F1}s";
	}

	public override void _ExitTree()
    {
		SignalBus.Instance.ToolChanged -= this.OnToolChanged;
		SignalBus.Instance.SeedChanged -= this.OnSeedChanged;
		this.timer.Timeout -= this.OnTimerTimeOut;
	}
}
