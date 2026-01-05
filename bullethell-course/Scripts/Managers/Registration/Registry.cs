using Godot;
using System;

namespace bullethellcourse.Scripts.Managers.Registration;

public partial class Registry : Node
{
    public override void _Ready()
    {
		_ = DummyManager.Instance;
		_ = BulletLoadManager.Instance;
		_ = ItemLoadManager.Instance;
		_ = EnemyLoadManager.Instance;
		_ = GameManager.Instance;
		//this.AddChild(new InputManager());
	}
}
