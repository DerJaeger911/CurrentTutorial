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
	private Vector2 lastDirection = new(0, 1);
	private int currentTool = 0;
	private string toolName;
	private PlayerAudio playerAudio;


	public override void _Ready()
	{
		base._Ready();

		this.playerAudio = (PlayerAudio)this.GetNode<Node>("Audio");

		this.toolName = ToolConstants.All[this.currentTool];

		this.CollisionLayer = LayerMask.PlayerLayer;
		this.CollisionMask = LayerMask.PlayerMask;

		this.animationTree = this.GetNode<AnimationTree>(AnimationPaths.AnimationTree);
		this.animationTree.Active = true;
		this.moveStateMachine = (AnimationNodeStateMachinePlayback)this.animationTree.Get(AnimationPaths.MsmPlayback);
		this.toolStateMachine = (AnimationNodeStateMachinePlayback)this.animationTree.Get(AnimationPaths.TsmPlayback);

		this.moveStateMachine.Travel("idle");
		this.animationTree.Set(AnimationPaths.MsmIdleBlend, this.lastDirection);

		this.animationTree.AnimationFinished += this.OnAnimationFinished;
	}

	protected override void GetInput()
	{

		this.direction = Input.GetVector("left", "right", "up", "down");

		this.action = Input.IsActionJustPressed("action");
		if (Input.IsActionJustPressed("tool_backward") || Input.IsActionJustPressed("tool_forward"))
		{
			int toggleDirection = (int)Input.GetAxis("tool_backward", "tool_forward");
			int nextIndex = Mathf.PosMod(this.currentTool + toggleDirection, ToolConstants.All.Length);
			this.currentTool = nextIndex;
			this.toolName = ToolConstants.All[this.currentTool];
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
			this.animationTree.Set(AnimationPaths.ToolStateMachine + "/" + this.toolName + AnimationPaths.BlendPosition, this.lastDirection);
		}

		if (this.direction != Vector2.Zero)
		{
			this.moveStateMachine.Travel("move");
			var newDirection = this.direction.Normalized().Round();
			if(newDirection != this.lastDirection)
			{
				this.animationTree.Set(AnimationPaths.MsmMoveBlend, newDirection);
				this.animationTree.Set(AnimationPaths.MsmIdleBlend, newDirection);
				this.lastDirection = newDirection;
			}
		}
		else
		{
			this.moveStateMachine.Travel("idle");
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		this.canMove = true;

		this.playerAudio.PlayAudio(this.toolName);
	}

	public override void _ExitTree()
	{
		this.animationTree.AnimationFinished -= this.OnAnimationFinished;
	}
}
