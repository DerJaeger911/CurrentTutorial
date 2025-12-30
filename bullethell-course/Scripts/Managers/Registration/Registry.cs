using Godot;
using System;

namespace bullethellcourse.Scripts.Managers.Registration;

public partial class Registry : Node
{
    public override void _Ready()
    {
        this.AddChild(new SingletonRegistry());
        this.AddChild(new RootTreeAdd());
    }
}
