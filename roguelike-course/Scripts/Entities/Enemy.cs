using Godot;
using RoguelikeCourse.Scripts;
using RoguelikeCourse.Scripts.Entities.Bases;
using RoguelikeCourse.Scripts.Manager.Signals;
using RoguelikeCourse.Scripts.Statics;

public partial class Enemy : Entity
{
    [Export]
    private double attackRate = 0.5;
    private double lastAttackTime;

    private Sprite2D sprite;

    private Room room;

    private bool isActive;

    private Player player;
    private Vector2 playerDirection;
    private float playerDistance;

    public override void _Ready()
    {
        base._Ready();
        this.CollisionLayer = LayerMasks.EnemyLayer;
        this.MoveSpeed = 20;
        this.sprite = this.GetNode<Sprite2D>("Sprite");

        GameSignals.Instance.PlayerEnteredRoom += this.OnPlayerEnteredRoom;


        this.CallDeferred(nameof(FindPlayer));
    }

    private void FindPlayer()
    {
        this.player = this.GetTree().GetFirstNodeInGroup("Player") as Player;
    }

    public void Initialize(Room inRoom)
    {
        this.isActive = false;
        this.room = inRoom;
    }

    private void OnPlayerEnteredRoom(Room playerRoom)
    {
        this.isActive = playerRoom == this.room;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (this.player is null || !this.isActive)
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
        double time = Time.GetUnixTimeFromSystem();
        if ( time - this.lastAttackTime < this.attackRate)
        {
            return;
        }
        this.lastAttackTime = time;
        this.player.TakeDamage(this.AttackDamage, this.player); 
    }

    protected override void Die()
    {
        GameSignals.Instance.EmitSignal(nameof(GameSignals.EnemyDefeated), this);
        this.QueueFree();
    }

	public override void _ExitTree()
    {
        GameSignals.Instance.PlayerEnteredRoom -= this.OnPlayerEnteredRoom;
    }
}