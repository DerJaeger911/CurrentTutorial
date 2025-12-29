using Godot;
using RoguelikeCourse.Scripts.Statics;
using System.Threading.Tasks;

namespace RoguelikeCourse.Scripts.Entities.Bases;

public abstract partial class Entity : CharacterBody2D, IAttacker
{

    [Export]
    private float moveSpeed = 50;
    private float initialMoveSpeed;

    [Export]
    private int maxHp = 4;
    private int initialMaxHp;
    private int currentHp;

    [Export]
    private int attackDamage = 1;
    private int initialAttackDamage;

    [Export]
    private float attackRange = 10;
    private float initialAttackRange;

    private AudioStreamPlayer2D damageSound;

    public override void _Ready()
    {
        this.initialMoveSpeed = this.moveSpeed;
        this.initialMaxHp = this.maxHp;
        this.CurrentHp = this.maxHp;
        this.initialAttackDamage = this.attackDamage;
        this.initialAttackRange = this.attackRange;
        this.CollisionMask = LayerMasks.EntityMask;

        this.damageSound = this.GetNode<AudioStreamPlayer2D>("DamageSound");
    }

    public float MoveSpeed
    {
        get => this.moveSpeed;
        set => this.moveSpeed =  value;
    }

    public int MaxHp
    {
		get => this.maxHp; set => this.maxHp = value;
	}

    public int AttackDamage 
    {
        get => this.attackDamage;
        set => this.attackDamage = value;
    }

    protected float AttackRange
    {
        get => this.attackRange;
        set => this.attackRange = this.attackRange == this.initialAttackRange ? value : this.attackRange;
    }
    public int CurrentHp { get => this.currentHp; set => this.currentHp = value; }

    public virtual void Heal(int amount)
    {
        if (this.CurrentHp + amount < this.maxHp)
        {
            this.CurrentHp += amount;
        }
        else
        {
            this.CurrentHp = this.maxHp;
        }
    }

    public virtual void TakeDamage(int amount, Node target)
    {
        GD.Print(amount);

        this.CurrentHp -= amount;

        this.damageSound.Play();

		_ = this.DamageFlashAsync();

		if (this.CurrentHp <= 0)
        {
            this.Die();
        }
    }

	private async Task DamageFlashAsync()
	{
		this.Visible = false;
		var timer = this.GetTree().CreateTimer(0.07f);
		await this.ToSignal(timer, "timeout");
		this.Visible = true;
		timer = this.GetTree().CreateTimer(0.1f);
		await this.ToSignal(timer, "timeout");
	}

	public void ResetStat(StatEnum stat, Entity entity)
    {
        if(entity is Player player)
        {
            switch (stat)
            {
                case StatEnum.ShootRate:
                    player.InternalStatResets(stat);
                    break;
            }
        }
    }

    protected abstract void Die();
}