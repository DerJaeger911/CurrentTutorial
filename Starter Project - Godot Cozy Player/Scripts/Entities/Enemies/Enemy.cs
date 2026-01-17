using Dcozysandbox.Scripts.Entities;
using Dcozysandbox.Scripts.Entities.Player;
using Dcozysandbox.Scripts.Helper;
using Dcozysandbox.Scripts.PhysicsLayers;
using Godot;
using System.Runtime.CompilerServices;

namespace Dcozysandbox.Scripts.Enemies;

public partial class Enemy : Entity
{
	private Player player;
	private Sprite2D sprite;
	private int pushDistance = 200;
	private AnimationPlayer animationPlayer;
	private bool isDead;
	

	public override void _Ready()
	{
		base._Ready();

		this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");
		this.sprite = this.GetNode<Sprite2D>("Sprite");
		this.animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");

		this.CollisionLayer = LayerMask.EnemyLayer;
		this.CollisionMask = LayerMask.EnemyMask;

		//Because "this.sprite.Material.ResourceLocalToScene = true;" not working
		if (this.sprite.Material != null)
		{
			this.sprite.Material = (Material)this.sprite.Material.Duplicate();
		}

		this.animationPlayer.AnimationFinished += this.OnAnimationFinishedAndDead;
	}



    protected override void GetDirection()
    {
		if(this.player is not null)
		{
			this.Direction = (this.player.Position - this.Position).Normalized();
		}
	}

	protected override void SetAnimation() { }

	public void TakeDamage(int damage)
	{
		ShaderHelper.Flash(this, this.sprite.Material);
		this.Push();

		this.Health -= damage;

		ShaderHelper.Flash(this, this.sprite.Material);

		if (this.Health <= 0)
		{
			this.Die();
		}
	}

	private void Push()
	{
		Tween tween = this.GetTree().CreateTween();
		Vector2 target = (this.player.Position - this.Position).Normalized() * -1 * this.pushDistance;
		tween.TweenProperty(this, nameof(this.PushDirection), target, 0.1);
		tween.TweenProperty(this, nameof(this.PushDirection), Vector2.Zero, 0.2);
	}

	public void Die()
	{
		this.Speed = 0;
		this.animationPlayer.CurrentAnimation = "explode";
		this.isDead = true;
	}

	private void OnAnimationFinishedAndDead(StringName animName)
	{
		if(!this.isDead)
		{
			return;
		}

		if (this.Position.DistanceTo(this.player.Position) < 12) 
		{
			this.player.Stun(5);
		}

		this.QueueFree();
		GD.Print("Es hat bumm gemacht.");
	}
}
