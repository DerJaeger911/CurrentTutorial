using Godot;

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

    public Entity()
    {
        this.initialMoveSpeed = this.moveSpeed;
        this.initialMaxHp = this.maxHp;
        this.currentHP = this.maxHp;
        this.initialAttackDamage = this.attackDamage;
        this.initialAttackRange = this.attackRange;
    }

    protected float MoveSpeed
    {
        get => this.moveSpeed;
        set => this.moveSpeed = this.moveSpeed == this.initialMoveSpeed ? value : this.moveSpeed;
    }

    protected float MaxHp
    {
        get => this.maxHp;
        set => this.maxHp = this.maxHp == this.initialMaxHp ? (int)value : this.maxHp;
    }

    protected float AttackDamage
    {
        get => this.attackDamage;
        set => this.attackDamage = this.attackDamage == this.initialAttackDamage ? (int)value : this.attackDamage;
    }

    protected float AttackRange
    {
        get => this.attackRange;
        set => this.attackRange = this.attackRange == this.initialAttackRange ? value : this.attackRange;
    }

    public void TakeDamage(int amaount)
    {

        if (this.currentHP <= 0)
        {
            this.Die();
        }
    }

    protected abstract void Die();
}