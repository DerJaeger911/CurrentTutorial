using Godot;
using GodotRTSCourse.Scripts.Game;
using GodotRTSCourse.Scripts.Game.EnumAndConsts;
using System;

namespace GodotRTSCourse.Scripts.Game
{
    public partial class UnitVisual : Sprite2D
    {
        private Unit unit;
        private Vector2 lastUnitPosition;

        public override void _Ready()
        {
            this.unit = this.GetParent<Unit>();
            this.unit.TakeDamage += this.DamageFlash;
            this.lastUnitPosition = this.unit.GlobalPosition;
        }

        public override void _Process(double delta)
        {
            if (this.unit.GlobalPosition != this.lastUnitPosition)
            {
                this.PlayMovementAnim();
            }
            else
            {
                this.Rotation = 0;
            }

            float direction = this.unit.GlobalPosition.X - this.lastUnitPosition.X;

            if (direction > 0)
            {
                this.FlipH = false;
            }
            else if (direction < 0)
            {
                this.FlipH = true;
            }

                this.lastUnitPosition = this.unit.GlobalPosition;
        }

        private async void DamageFlash(int health)
        {
            this.Modulate = Colors.Red;
            await this.ToSignal(this.GetTree().CreateTimer(0.05f), SceneTreeTimer.SignalName.Timeout);
            this.Modulate = Colors.White;
        }

        private void PlayMovementAnim()
        {
            float time = (float)Time.GetTicksMsec() / 100;
            var r = MathF.Sin(time) * 5;

            this.Rotation = Mathf.DegToRad(r);
        }

        public override void _ExitTree()
        {
            this.unit.TakeDamage -= this.DamageFlash;
        }
    }
}
