using Godot;
using System;

namespace Adventuregame.Scripts.Characters;

public partial class Character : CharacterBody3D
{
	[Export]
	private float baseSpeed = 4;

    public Single BaseSpeed { get => this.baseSpeed; set => this.baseSpeed = value; }
    public Vector2 MovementInput { get; set; }
}
