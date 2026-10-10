using Godot;
using Metroidvania.game.interfaces;
public class Shotgun
{
    public void EmitParticles(Vector2 direction, Vector2 spawnOffset, GpuParticles2D shotgunParticles)
    {
        shotgunParticles.ProcessMaterial.Set("direction", direction);
        shotgunParticles.GlobalPosition = spawnOffset;
        shotgunParticles.Emitting = true;
    }

    public void OnShotgunHit(Node2D caller, Vector2 direction)
    {
        foreach (Node2D body in caller.GetTree().GetNodesInGroup("Damageable"))
        {
            Vector2 callerPosition = caller.GlobalPosition;
            float aimAngle = Mathf.RadToDeg(direction.Angle());
            float bodyAngle = Mathf.RadToDeg((body.GlobalPosition - callerPosition).Angle());

            if (Mathf.Abs(aimAngle - bodyAngle) < 90 && 
            body != caller && 
            body is IDamageable entity &&
            callerPosition.DistanceTo(body.GlobalPosition) < 150f)
            {
                entity.Hit();
            }
        }
    }
}