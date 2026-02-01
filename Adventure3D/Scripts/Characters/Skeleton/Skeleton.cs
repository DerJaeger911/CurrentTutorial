using Adventuregame.Scripts.Characters;
using Adventuregame.Scripts.Characters.Player;
using Godot;
using Godot.NativeInterop;
using System;

public partial class Skeleton : Character
{
    [Export]
    private float noticeRadius = 30;
    [Export]
    private float speed = 3;

	private Player player;

    public override void _Ready()
    {
        base._Ready();
        this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");
    }

    override public void _PhysicsProcess(Double delta)
    {
        base._PhysicsProcess(delta);
        this.MoveToPlayer(delta);
        this.MoveAndSlide();
    }

    private void MoveToPlayer(double delta)
    {
        if(this.Position.DistanceTo(this.player.Position) < this.noticeRadius)
        {
            Vector3 targetDir = (this.player.Position - this.Position).Normalized();
            Vector2 targetVector2 = new Vector2(targetDir.X, targetDir.Z);
			var targetAngle = -targetVector2.Angle() + float.Pi / 2;
            this.Rotation= new Vector3(this.Rotation.X, Mathf.RotateToward(this.Rotation.Y, targetAngle, (float)delta * 6), this.Rotation.Z);
			this.Velocity = new Vector3(targetDir.X, 0, targetDir.Z) * this.speed;
        }
    }
}