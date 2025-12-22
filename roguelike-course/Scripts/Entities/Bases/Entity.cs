using Godot;
using RoguelikeCourse.Scripts.Statics;

namespace RoguelikeCourse.Scripts.Entities.Bases;

public abstract partial class Entity : CharacterBody2D
{

    [Export]
    private float moveSpeed = 50;
    private float initialMoveSpeed;

    [Export]
    private int maxHp = 4;
    private int initialMaxHp;
    private int currentHP;

    [Export]
    private int attackDamage = 1;
    private int initialAttackDamage;

    [Export]
    private float attackRange = 10;
    private float initialAttackRange;

    public override void _Ready()
    {
        this.initialMoveSpeed = this.moveSpeed;
        this.initialMaxHp = this.maxHp;
        this.CurrentHP = this.maxHp;
        this.initialAttackDamage = this.attackDamage;
        this.initialAttackRange = this.attackRange;
        this.CollisionMask = LayerMasks.EntityMask;
    }

    public float MoveSpeed
    {
        get => this.moveSpeed;
        set => this.moveSpeed =  value;
    }

    protected float MaxHp
    {
        get => this.maxHp;
        set => this.maxHp = this.maxHp == this.initialMaxHp ? (int)value : this.maxHp;
    }

    public int AttackDamage
    {
        get => this.attackDamage;
        set => this.attackDamage = this.attackDamage == this.initialAttackDamage ? (int)value : this.attackDamage;
    }

    protected float AttackRange
    {
        get => this.attackRange;
        set => this.attackRange = this.attackRange == this.initialAttackRange ? value : this.attackRange;
    }
    public int CurrentHP { get => this.currentHP; set => this.currentHP = value; }

    public void Heal(int amount)
    {
        if (this.CurrentHP + amount < this.maxHp)
        {
            this.CurrentHP += amount;
        }
        else
        {
            this.CurrentHP = this.maxHp;
        }
    }

    public void TakeDamage(int amaount, Node target)
    {
        this.CurrentHP -= amaount;

        if (this.CurrentHP <= 0)
        {
            this.Die();
        }
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