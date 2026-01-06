using bullethellcourse.Scripts.Bullets;
using bullethellcourse.Scripts.Pools;
using bullethellcourse.Scripts.Statics;
using Godot;
using System.Threading.Tasks;

namespace bullethellcourse.Scripts.Entities;

public abstract partial class Entity : CharacterBody2D
{
    [Export]
    private float maxSpeed = 100;
    [Export]
	protected float acceleration = 0.2f;
    [Export]
    private double shootRate = 0.1f;
    protected abstract int MaxHp { get; set; }
	protected int CurrentHp { get; set; }

	protected bool isMoving;

	protected double lastShootTime;

	protected Node2D muzzle;

	private Bullet bullet;

	protected Sprite2D sprite;

	private ProgressBar healthbar;

	protected abstract BulletPool BulletPool { get; }

	public float AdditionalBulletSpeed { get; set; }

	protected abstract float moveWobbleAmount { get; }

	protected abstract Color FlashColor { get; }

	public abstract EntityTypeEnum BulletOwner { get; }
    public double ShootRate { get => this.shootRate; set => this.shootRate = value; }
	public float MaxSpeed { get => this.maxSpeed; set => this.maxSpeed = value; }

    public override void _Ready()
	{
		this.CollisionMask = LayerMask.EntityMask;
		this.muzzle = this.GetNode<Node2D>("Muzzle");
		this.sprite = this.GetNode<Sprite2D>("Sprite");
		this.healthbar = this.GetNode<ProgressBar>("HealthBar");
		this.healthbar.MaxValue = this.MaxHp;
		this.ResetHealth();
	}

	protected void ResetHealth()
	{
		this.CurrentHp = this.MaxHp;
		this.healthbar.Value = this.CurrentHp;
	}

	public override void _Process(System.Double delta)
	{
		this.FlipH();
		this.MoveWobble();

	}

	public override void _PhysicsProcess(System.Double delta)
	{

	}

	protected void Shoot(BulletTypeEnum bulletType)
	{
		if (Time.GetUnixTimeFromSystem() - this.lastShootTime > this.ShootRate)
		{
			this.lastShootTime = Time.GetUnixTimeFromSystem();

			this.InstantiateBullet(bulletType);

			this.bullet.AdditionalSpeed = this.AdditionalBulletSpeed;
			this.bullet.MoveDirection = this.BulletDirection();
			this.PlayShootSound();
		}
	}

	private void MoveWobble()
	{
		if (!this.isMoving)
		{
			this.sprite.RotationDegrees = 0;
			return;
		}

		double t = Time.GetUnixTimeFromSystem();
		double rotation = Mathf.Sin(t * 20) * this.moveWobbleAmount;

		this.sprite.RotationDegrees = (float)rotation;
	}

	private void InstantiateBullet(BulletTypeEnum bulletType)
	{
		Node parent = this.GetTree().CurrentScene;
		this.bullet = (Bullet)this.BulletPool.Spawn(parent);
		this.bullet.Init(this.BulletOwner, bulletType);
		this.bullet.GlobalPosition = this.muzzle.GlobalPosition;
	}

	public virtual void TakeDamage(int damage)
	{
		if (this.CurrentHp <= 0)
		{
			this.Die();
		}
		else
		{
			this.CurrentHp -= damage;
			this.healthbar.Value = this.CurrentHp;
			_ = this.DamageFlash();
			this.PlayDamageSound();
		}
	}

	protected async Task DamageFlash()
	{
		this.sprite.Modulate = this.FlashColor;

		var timer = this.GetTree().CreateTimer(0.05f);
		await this.ToSignal(timer, "timeout");

		this.sprite.Modulate = Colors.White;
	}

	public void Heal(int amount)
	{
		this.CurrentHp += amount;

		if(this.CurrentHp > this.MaxHp)
		{
			this.CurrentHp = this.MaxHp;
		}
		this.healthbar.Value = this.CurrentHp;
	}

	protected abstract void Die();

	protected abstract void PlayShootSound();

	protected abstract void PlayDamageSound();

	protected abstract Vector2 BulletDirection();

	protected abstract void FlipH();
}
