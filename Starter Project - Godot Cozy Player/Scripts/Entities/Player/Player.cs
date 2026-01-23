using Dcozysandbox.Scripts.AutoLoads.Busses;
using Dcozysandbox.Scripts.Constants;
using Dcozysandbox.Scripts.Constants.Paths;
using Dcozysandbox.Scripts.Enums;
using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;
using System.Runtime.CompilerServices;

namespace Dcozysandbox.Scripts.Entities.Player;

public partial class Player : Entity
{
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback moveStateMachine;
	private AnimationNodeStateMachinePlayback toolStateMachine;
	private string toolName;
	private PlayerAudio playerAudio;
	private bool action;
	private bool plant;
	private bool canAct = true;
	private bool canFish;
	private Timer fishDelayTimer;
	private bool animationFinished = true;
	private bool fishingAction;
	private FishGame fishGame;
	private Timer plantTimer;
	private BuildOverlay buildOverlay;


	[Export]
	public int ToolOffset { get; set; } = 20;
	public int CurrentTool { get; set; } = 0;

	public bool IsBuilding {  get; set; }

	public SeedEnum CurrentSeed { get; set; } = SeedEnum.Corn;
	public Vector2 LastDirection { get; set; } = new(0, 1);

	protected override int MaxHealth { get; set; } = 10;
	public Timer StunTimer { get; set; }

	public bool IsFishing { get; set; }

	public override void _Ready()
	{
		base._Ready();

		this.playerAudio = (PlayerAudio)this.GetNode<Node>("Audio");

		this.toolName = ToolConstants.All[this.CurrentTool];

		this.fishDelayTimer = this.GetNode<Timer>("Timer/FishDelayTimer");

		this.plantTimer = this.GetNode<Timer>("Timer/PlantTimer");

		this.fishGame = this.GetNode<FishGame>("FishGameContainer/FishGame");

		this.buildOverlay = this.GetNode<BuildOverlay>("../../Overlay/BuildOverlay");

		this.CollisionLayer = LayerMask.PlayerLayer;
		this.CollisionMask = LayerMask.PlayerMask;

		this.animationTree = this.GetNode<AnimationTree>(AnimationPaths.AnimationTree);
		this.animationTree.Active = true;
		this.moveStateMachine = (AnimationNodeStateMachinePlayback)this.animationTree.Get(AnimationPaths.MsmPlayback);
		this.toolStateMachine = (AnimationNodeStateMachinePlayback)this.animationTree.Get(AnimationPaths.TsmPlayback);

		this.moveStateMachine.Travel("idle");
		this.animationTree.Set(AnimationPaths.MsmIdleBlend, this.LastDirection);

		this.animationTree.AnimationFinished += this.OnAnimationFinished;
		SignalBus.Instance.CanFish += this.OnCanFish;
		this.plantTimer.Timeout += this.OnPlantTimerTimeout;
		
	}

	public override void _Input(InputEvent @event)
	{
		this.ToggleTool();
		this.ChangeToolWithKey(@event);
		this.ToggleSeed();

		if (this.canAct)
		{
			this.action = Input.IsActionPressed("action");
			this.canAct = !this.action;
			if (Input.IsActionJustPressed("plant"))
			{
				var interactionPosition = this.Position + this.LastDirection * this.ToolOffset;
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.SeedInteract, (int)this.CurrentSeed, interactionPosition);
				this.PlayerCantAct();
				this.plantTimer.Start();
			}
		}

		if (this.IsFishing && !this.canAct)
		{
			this.fishingAction = Input.IsActionJustPressed("action");
		}

		if (Input.IsActionJustPressed("build"))
		{
			this.PlayerCantAct();
			this.IsBuilding = true;
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.BuildMode);
			this.animationTree.Set(AnimationPaths.MsmIdleBlend, new Vector2 (0, 1));
		}

		if (this.IsBuilding && Input.IsActionJustPressed("ui_cancel"))
		{
			this.PlayerCanAct();
			this.IsBuilding = false;
			this.buildOverlay.UnReveal();
		}
	}

	private void ToggleTool()
	{
		float toggleDirection = Input.GetAxis("tool_backward", "tool_forward");
		if (!Mathf.IsZeroApprox(toggleDirection))
		{
			int nextIndex = Mathf.PosMod(this.CurrentTool + (int)toggleDirection, ToolConstants.All.Length);
			this.ChangeTool(nextIndex);
		}
	}

	private void ChangeToolWithKey(InputEvent @event)
	{
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
	}

	private void ToggleSeed()
	{
		if (Input.IsActionJustPressed("seed_toggle"))
		{
			int enumSize = System.Enum.GetValues(typeof(SeedEnum)).Length;
			this.CurrentSeed = (SeedEnum)Mathf.PosMod((int)this.CurrentSeed + 1, enumSize);
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.SeedChanged, (int)this.CurrentSeed);
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
	}

	protected override void GetDirection()
	{
		if (this.CanMove)
		{
			this.Direction = Input.GetVector("left", "right", "up", "down");
		}
	}

	protected override void SetAnimation()
	{
		if (this.action)
		{
			this.toolStateMachine.Travel(this.toolName);
			this.animationTree.Set(AnimationPaths.OsRequest, (int)AnimationNodeOneShot.OneShotRequest.Fire);
			this.CanMove = false;
			this.Direction = Vector2.Zero;
			this.action = false;
			this.animationTree.Set(AnimationPaths.ToolStateMachine + "/" + this.toolName + AnimationPaths.BlendPosition, this.LastDirection);
			if (this.toolName == ToolConstants.Fish)
			{
				this.IsFishing = true;
			}
		}

		if (this.Direction != Vector2.Zero)
		{
			this.moveStateMachine.Travel("move");
			Vector2 newDirection = this.Direction.Normalized().Round();
			if (newDirection != this.LastDirection)
			{
				this.animationTree.Set(AnimationPaths.MsmMoveBlend, newDirection);
				this.animationTree.Set(AnimationPaths.MsmIdleBlend, newDirection);
				this.LastDirection = newDirection;
				this.animationTree.Set(AnimationPaths.FBSBlend, newDirection);
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

		if (!this.CanMove && this.IsFishing)
		{
			this.GetFishingInput();
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		if (!this.IsFishing || !this.canFish)
		{
			this.StopFishing();
		}
		else
		{
			this.animationTree.Set(AnimationPaths.FishBlendAmount, 1);
			this.fishGame.Visible = true;
			this.PlayerCantAct();
		}
		this.animationFinished = true;
	}

	private void PlayerCantAct()
	{
		this.CanMove = false;
		this.canAct = false;
		this.Direction = Vector2.Zero;
	}

	private void PlayerCanAct()
	{
		this.CanMove = true;
		this.canAct = true;
	}

	public void Stun(float stunTime)
	{
		this.PlayerCantAct();

		if (this.StunTimer != null)
		{
			this.StunTimer.Stop();
			this.StunTimer.QueueFree();
		}

		this.StunTimer = new Timer();
		this.AddChild(this.StunTimer);

		this.StunTimer.WaitTime = stunTime;
		this.StunTimer.OneShot = true;

		this.StunTimer.Timeout += () =>
		{
			this.PlayerCanAct();
			this.StunTimer.QueueFree();
			this.StunTimer = null;
		};

		this.StunTimer.Start();
	}

	private void OnPlantTimerTimeout()
	{
		this.PlayerCanAct();
	}



	private void OnCanFish(bool canFish)
	{
		this.canFish = canFish;
		this.PlayerCantAct();

		if (this.IsFishing && !this.canFish)
		{
			this.IsFishing = false;
		}
	}

	private void StopFishing()
	{
		this.IsFishing = false;
		this.PlayerCanAct();
		this.animationTree.Set(AnimationPaths.FishBlendAmount, 0);
		this.fishGame.Visible = false;
	}

	private void GetFishingInput()
	{
		if (this.Direction != Vector2.Zero && this.fishDelayTimer.TimeLeft == 0)
		{
			this.StopFishing();
		}
		if (this.fishingAction)
		{
			this.fishingAction = false;
			this.fishGame.GetFish();
			this.StopFishing();
		}
	}



	public override void _ExitTree()
	{
		this.animationTree.AnimationFinished -= this.OnAnimationFinished;
		SignalBus.Instance.CanFish -= this.OnCanFish;
		this.plantTimer.Timeout -= this.OnPlantTimerTimeout;
	}
}