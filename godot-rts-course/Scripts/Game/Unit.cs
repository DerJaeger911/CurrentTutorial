using Godot;

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

    public TeamEnums Team { get => team; set => team = value; }

    public override void _Ready()
    {
        currentHealth = maxHealth;

        agent = GetNode<NavigationAgent2D>(GodotConsts.NavAgent);
    }

    public override void _Process(double delta)
    {
        if (!agent.IsTargetReached())
        {
            Move(delta);
        }
    }

    private void Move(double delta)
    {
        Vector2 movePosition = agent.GetNextPathPosition();
        Vector2 moveDir = GlobalPosition.DirectionTo(movePosition);
        Vector2 movement = moveDir * moveSpeed * (float)delta;
    
        Translate(movement);
    }

    private void TargetCheck()
    {

    }

    private void TryAttackTarget()
    {

    }

	public void SetMoveToTarget(Vector2 target)
    {
        agent.TargetPosition = target;
        attackTarget = null;
    }

    public void SetAttackTarget(Unit target)
    {

    }

    private void OnTakeDamage(int amount)
    {

    }

    private void Die()
    {
        QueueFree();
    }
}
