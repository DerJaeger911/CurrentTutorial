using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Constants.Paths;
using Dcozysandbox.Scripts.Enums;
using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;
using System;

namespace Dcozysandbox.Scripts.Player;
public partial class Player : Entity
{
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback moveStateMachine;
	private AnimationNodeStateMachinePlayback toolStateMachine;
	private string toolName;
	private PlayerAudio playerAudio;
	private bool action;

	[Export]
	public int ToolOffset { get; set; } = 20;
	public int CurrentTool { get; set; } = 0;
    public Vector2 LastDirection { get; set; } = new(0, 1);

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
		int nextIndex;
		if (@event is InputEventKey keyEvent &&
	keyEvent.Pressed &&
	!keyEvent.Echo &&
	keyEvent.Keycode >= Key.Key1 &&
	keyEvent.Keycode <= Key.Key5)
		{
			switch (keyEvent.Keycode)
			{
				case Key.Key1:
					nextIndex = 0;
					break;
				case Key.Key2:
					nextIndex = 1;
					break;
				case Key.Key3:
					nextIndex = 2;
					break;
				case Key.Key4:
					nextIndex = 3;
					break;
				case Key.Key5:
					nextIndex = 4;
					break;
				default:
					nextIndex = this.CurrentTool;
					break;
			}
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.ToolChanged, nextIndex);
			this.SetTool(nextIndex);
		}
	}

	private void SetTool(int index)
	{
		this.CurrentTool = index;
		this.toolName = ToolConstants.All[this.CurrentTool];
		GD.Print($"{this.toolName} is {this.CurrentTool}");
	}

	protected override void GetDirection()
	{

		this.direction = Input.GetVector("left", "right", "up", "down");
	}

    public override void _Process(Double delta)
    {
        base._Process(delta);

		this.action = Input.IsActionJustPressed("action");
		if (Input.IsActionJustPressed("tool_backward") || Input.IsActionJustPressed("tool_forward"))
		{
			int toggleDirection = (int)Input.GetAxis("tool_backward", "tool_forward");
			int nextIndex = Mathf.PosMod(this.CurrentTool + toggleDirection, ToolConstants.All.Length);
			this.SetTool(nextIndex);
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.ToolChanged, nextIndex);
		}
	}

	protected override void SetAnimation()
	{
		if (this.action)
		{
			this.toolStateMachine.Travel(this.toolName);
			this.animationTree.Set(AnimationPaths.OsRequest, (int)AnimationNodeOneShot.OneShotRequest.Fire);
			this.canMove = false;
			this.action = false;
			this.animationTree.Set(AnimationPaths.ToolStateMachine + "/" + this.toolName + AnimationPaths.BlendPosition, this.LastDirection);
		}

		if (this.direction != Vector2.Zero)
		{
			this.moveStateMachine.Travel("move");
			Vector2 newDirection = this.direction.Normalized().Round();
			if(newDirection != this.LastDirection)
			{
				this.animationTree.Set(AnimationPaths.MsmMoveBlend, newDirection);
				this.animationTree.Set(AnimationPaths.MsmIdleBlend, newDirection);
				this.LastDirection = newDirection;
			}
			this.playerAudio.PlayWalkSound();
		}
		else
		{
			this.moveStateMachine.Travel("idle");
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		this.canMove = true;
	}

	public override void _ExitTree()
	{
		this.animationTree.AnimationFinished -= this.OnAnimationFinished;
	}
}
