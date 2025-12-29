using Godot;

namespace RoguelikeCourse.Scripts.Entities.Bases
{
    internal partial class Boss : Enemy
    {
        protected override void Die()
        {
            GD.Print("Boss ded");
            base.Die();
        }
    }
}
