using FarmingRpg.Scripts.Consts;
using Godot;

namespace FarmingRpg.Scripts;

public partial class Player : CharacterBody2D
{
    [Export]
    private float movementSpeed = 30;
    private Vector2 facingDirection;

    private AnimatedSprite2D animatedSprite;

    public override void _Ready()
    {
        this.facingDirection = Vector2.Down;
        this.animatedSprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 moveInput = Input.GetVector(PlayerInputConsts.MoveLeft, PlayerInputConsts.MoveRight, PlayerInputConsts.MoveUp, PlayerInputConsts.MoveDown); 

        if (moveInput != Vector2.Zero)
        {
            this.facingDirection = moveInput;
        }

        this.Velocity = moveInput * this.movementSpeed;
        this.MoveAndSlide();
        this.Animate();
    }

    private void Animate()
    {
        string state = this.Velocity.Length() > 0 ? AnimationConsts.Walk : AnimationConsts.Idle;
        string direction;

        if (Mathf.Abs(this.facingDirection.X) > Mathf.Abs(this.facingDirection.Y))
        {
            if (this.facingDirection.X > 0)
            {
                direction = AnimationConsts.FaceRight;
            }
            else
            {
                direction = AnimationConsts.FaceLeft;
            }
        }
        else
        {
            if (this.facingDirection.Y > 0)
            {
                direction = AnimationConsts.FaceDown;
            }
            else
            {
                direction = AnimationConsts.FaceUp;
            }
        }

        string animationName = $"{state}_{direction}";

        this.animatedSprite.Play(animationName);
    }
}
