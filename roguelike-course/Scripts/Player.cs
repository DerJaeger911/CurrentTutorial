using Godot;
using System;

namespace RoguelikeCourse.Scripts;

public partial class Player : CharacterBody2D
{
	[Export]
	private float moveSpeed = 50;

	private Sprite2D sprite;
	private Node2D weaponOrigin;
	private Node2D muzzle;

	public override void _Ready()
	{
		this.sprite = this.GetNode<Sprite2D>("Sprite");
		this.weaponOrigin = this.GetNode<Node2D>("Weapon");
		this.muzzle = this.GetNode<Node2D>("Weapon/Muzzle");
	}

	public override void _PhysicsProcess(Double delta)
	{
		Vector2 moveInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		this.Velocity = moveInput * this.moveSpeed;
		this.MoveAndSlide();
	}

	public override void _Process(Double delta)
	{
		Vector2 mousePosition = this.GetGlobalMousePosition();
		Vector2 mouseDirection = (mousePosition - this.GlobalPosition).Normalized();
		this.weaponOrigin.RotationDegrees = Mathf.RadToDeg(mouseDirection.Angle()) + 90;

		this.sprite.FlipH = mouseDirection.X > 0;
	}
}