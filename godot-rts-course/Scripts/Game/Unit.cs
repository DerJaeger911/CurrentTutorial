using Godot;
using GodotRTSCourse.Scripts.Game.EnumAndConsts;

namespace GodotRTSCourse.Scripts.Game;

public partial class Unit : Area2D
{
    [Signal]
    public delegate void TakeDamageEventHandler(int health);
    [Signal]
    public delegate void DeathSignalEventHandler(Unit unit);

    [Export]
    private float moveSpeed = 10;
    [Export]
    private int maxHealth = 20;
    private int currentHealth;

    [Export]
    private float attackRange = 20;
    [Export]
    private float attackRate = 0.5f;
    [Export]
    private int attackDamage = 5;

    private float lastAttackTime;

    [Export]
    private TeamEnums team;

    private Unit attackTarget;

    private NavigationAgent2D agent;

    public TeamEnums Team { get => this.team; set => this.team = value; }
    public int MaxHealth { get => this.maxHealth; set => this.maxHealth = value; }

    public override void _Ready()
    {
        this.currentHealth = this.MaxHealth;

        this.agent = this.GetNode<NavigationAgent2D>(GodotConsts.NavAgent);
    }

    public override void _Process(double delta)
    {
        if (!this.agent.IsNavigationFinished())
        {
            this.Move(delta);
        }

        this.TargetCheck();
    }

    private void Move(double delta)
    {
        Vector2 movePosition = this.agent.GetNextPathPosition();
        Vector2 moveDir = this.GlobalPosition.DirectionTo(movePosition);
        Vector2 movement = moveDir * this.moveSpeed * (float)delta;

        this.Translate(movement);
    }

    private void TargetCheck()
    {
        if (this.attackTarget == null || !IsInstanceValid(this.attackTarget))
        {
            this.attackTarget = null;
            return;
        }

        float dist = this.GlobalPosition.DistanceTo(this.attackTarget.GlobalPosition);

        if (dist <= this.attackRange)
        {
            this.agent.TargetPosition = this.GlobalPosition;
            this.TryAttackTarget();
        }
        else
        {
            this.agent.TargetPosition = this.attackTarget.GlobalPosition;
        }
    }

    private void TryAttackTarget()
    {
        float time = (float)Time.GetTicksMsec();

        if (time - this.lastAttackTime < this.attackRate * 1000)
        {
            return;
        }

        this.lastAttackTime = time;
        this.attackTarget.OnTakeDamage(this.attackDamage);
    }

    public void SetMoveToTarget(Vector2 target)
    {
        this.agent.TargetPosition = target;
        this.attackTarget = null;
    }

    public void SetAttackTarget(Unit target)
    {
        if (target.Team == this.Team)
        {
            return;
        }

        this.attackTarget = target;
    }

    private void OnTakeDamage(int amount)
    {
        this.currentHealth -= amount;
        this.EmitSignal(SignalName.TakeDamage, this.currentHealth);

        if (this.currentHealth <= 0)
        {
            this.Die();
        }
    }

    private void Die()
    {
        this.EmitSignal(SignalName.DeathSignal, this);
        this.QueueFree();
    }
}
