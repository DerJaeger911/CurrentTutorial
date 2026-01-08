using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Constants.Paths;
using Dcozysandbox.Scripts.Enums;
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


	public override void _Ready()
	{
		base._Ready();

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
		}
	}

	protected override void SetAnimation()
	{
		if (this.action)
		{
			this.toolStateMachine.Travel(ToolConstants.All[this.currentTool]);
			this.animationTree.Set(AnimationPaths.OsRequest, (int)AnimationNodeOneShot.OneShotRequest.Fire);
			this.canMove = false;
			this.action = false;
		}

		if (this.direction != Vector2.Zero)
		{
			this.moveStateMachine.Travel("move");
			this.lastDirection = this.direction.Sign();

			this.animationTree.Set(AnimationPaths.MsmIdleBlend, this.lastDirection);
			this.animationTree.Set(AnimationPaths.MsmMoveBlend, this.lastDirection);

			foreach (string tool in ToolConstants.All)
			{
				this.animationTree.Set(AnimationPaths.ToolStateMachine + "/" + tool + AnimationPaths.BlendPosition, this.lastDirection);
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
	}

	public override void _ExitTree()
	{
		this.animationTree.AnimationFinished -= this.OnAnimationFinished;
	}
}
