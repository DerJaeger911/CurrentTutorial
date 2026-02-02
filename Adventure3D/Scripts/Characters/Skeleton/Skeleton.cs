using Adventuregame.Scripts.Characters;
using Adventuregame.Scripts.Characters.Player;
using Adventuregame.Scripts.GlobalData;
using Adventuregame.Scripts.GlobalData.Enums;
using Godot;
using System;

public partial class Skeleton : Character
{
    [Export]
    private float noticeRadius = 30;
    [Export]
    private float speed = 3;

	private Player player;
    private AnimationTree animationTree;
    private Node3D skins;
	private readonly Random range = new Random();
    private float attackRadius = 3;
	private WeaponEnum randomWeapon;


	public override void _Ready()
    {
        base._Ready();
        this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");
        Node3D skinsNode = this.GetNode<Node3D>("Skins");

		Godot.Collections.Array<Node> skins = skinsNode.GetChildren();
        foreach (Node3D child in skins) 
        {
            child.Hide();
        }

        Node3D skin = (Node3D)skins.PickRandom();
		skin.Show();

        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.animationTree.AnimPlayer = $"../Skins/{skin.Name}/AnimationPlayer";

        this.randomWeapon = (WeaponEnum)this.range.Next(this.AllWeapons.Length);
		var selectWeapon = Global.Instance.Weapons[this.randomWeapon];
        this.Equip(selectWeapon, skin.GetNode<BoneAttachment3D>("Rig/Skeleton3D/RightHand"));
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
            this.SetMoveState("Running_A");
        }
    }
}