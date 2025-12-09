using Godot;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Entities.Bases;
using RoguelikeCourse.Scripts.Statics;

public partial class Enemy : Entity
{
    [Export]
    private float attackRate = 0.5f;
    private float lastAttackTime;

    private Sprite2D sprite;

    private Room room;

    private bool isActive;

    private Player player;
    private Vector2 playerDirection;
    private float playerDistance;

    public override void _Ready()
    {
        this.CollisionLayer = LayerMasks.EnemyLayer;
        this.CollisionMask = LayerMasks.EnemyMasks;
        this.MoveSpeed = 20;
        this.sprite = this.GetNode<Sprite2D>("Sprite");
        base._Ready();
        this.CallDeferred(nameof(FindPlayer));
    }

    private void FindPlayer()
    {
        this.player = this.GetTree().GetFirstNodeInGroup("Player") as Player;
    }

    private void Initialize()
    {

    }

    private void OnPlayerEnterRoom(Room playerRoom)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        if (this.player is null)
        {
            return;
        }
        this.playerDirection = this.GlobalPosition.DirectionTo(this.player.GlobalPosition);
        this.playerDistance = this.GlobalPosition.DistanceTo(this.player.GlobalPosition);
        this.sprite.FlipH = this.playerDirection.X < 0;
        this.Velocity = this.playerDirection * this.MoveSpeed;

        if (this.playerDistance <= this.AttackRange)
        {
            this.TryAttack();
            return;
        }

        this.MoveAndSlide();
    }

    private void TryAttack()
    {

    }

    protected override void Die()
    {
        this.QueueFree();
    }
}