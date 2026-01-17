using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Constants.Paths;
using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;

namespace Dcozysandbox.Scripts.Entities.Player;
public partial class Player : Entity
{
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback moveStateMachine;
	private AnimationNodeStateMachinePlayback toolStateMachine;
	private string toolName;
	private PlayerAudio playerAudio;
	private bool action;
	private bool canAct = true;

	[Export]
	public int ToolOffset { get; set; } = 20;
	public int CurrentTool { get; set; } = 0;
    public Vector2 LastDirection { get; set; } = new(0, 1);

	protected override int MaxHealth { get; set; } = 10;

	public override void _Ready()
	{
		base._Ready();

		this.playerAudio = (PlayerAudio)this.GetNode<Node>("Audio");

		this.toolName = ToolConstants.All[this.CurrentTool];

		this.CollisionLayer = LayerMask.PlayerLayer;
		this.CollisionMask = LayerMask.PlayerMask;

		this.animationTree = this.GetNode<AnimationTree>(AnimationPaths.AnimationTree);
		this.animationTree.Active = true;
		this.moveStateMachine = (AnimationNodeStateMachinePlayback)this.animationTree.Get(AnimationPaths.MsmPlayback);
		this.toolStateMachine = (AnimationNodeStateMachinePlayback)this.animationTree.Get(AnimationPaths.TsmPlayback);

		this.moveStateMachine.Travel("idle");
		this.animationTree.Set(AnimationPaths.MsmIdleBlend, this.LastDirection);

		this.animationTree.AnimationFinished += this.OnAnimationFinished;
	}

	public override void _Input(InputEvent @event)
	{
		float toggleDirection = Input.GetAxis("tool_backward", "tool_forward");
		if (!Mathf.IsZeroApprox(toggleDirection))
		{
			int nextIndex = Mathf.PosMod(this.CurrentTool + (int)toggleDirection, ToolConstants.All.Length);
			this.ChangeTool(nextIndex);
		}

		if (@event is InputEventKey { Pressed: true, Echo: false } keyEvent)
		{
			if (keyEvent.Keycode >= Key.Key1 && keyEvent.Keycode <= Key.Key5)
			{
				int nextIndex = (int)(keyEvent.Keycode - Key.Key1);

				if (nextIndex < ToolConstants.All.Length)
				{
					this.ChangeTool(nextIndex);
				}
			}
		}

		if (this.canAct)
		{
			this.action = Input.IsActionPressed("action");
			this.canAct = !this.action;
		}
	}

	private void ChangeTool(int nextIndex)
	{
		if (this.CurrentTool == nextIndex)
		{
			return;
		}

		this.SetTool(nextIndex);
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ToolChanged, nextIndex);
	}

	private void SetTool(int index)
	{
		this.CurrentTool = index;
		this.toolName = ToolConstants.All[this.CurrentTool];
		GD.Print($"{this.toolName} is {this.CurrentTool}");
	}

	protected override void GetDirection()
	{

		this.Direction = Input.GetVector("left", "right", "up", "down");
	}

	protected override void SetAnimation()
	{
		if (this.action)
		{
			this.toolStateMachine.Travel(this.toolName);
			this.animationTree.Set(AnimationPaths.OsRequest, (int)AnimationNodeOneShot.OneShotRequest.Fire);
			this.CanMove = false;
			this.action = false;
			this.animationTree.Set(AnimationPaths.ToolStateMachine + "/" + this.toolName + AnimationPaths.BlendPosition, this.LastDirection);
		}

		if (this.Direction != Vector2.Zero)
		{
			this.moveStateMachine.Travel("move");
			Vector2 newDirection = this.Direction.Normalized().Round();
			if(newDirection != this.LastDirection)
			{
				this.animationTree.Set(AnimationPaths.MsmMoveBlend, newDirection);
				this.animationTree.Set(AnimationPaths.MsmIdleBlend, newDirection);
				this.LastDirection = newDirection;
			}
			if (this.CanMove)
			{
				this.playerAudio.PlayWalkSound();
			}
		}
		else
		{
			this.moveStateMachine.Travel("idle");
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		this.CanMove = true;
		this.canAct = true;
	}

	public override void _ExitTree()
	{
		this.animationTree.AnimationFinished -= this.OnAnimationFinished;
	}
}
