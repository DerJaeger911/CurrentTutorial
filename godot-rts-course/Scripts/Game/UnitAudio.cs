using Godot;
using GodotRTSCourse.Scripts.Game;
using GodotRTSCourse.Scripts.Game.EnumAndConsts;
using System;

namespace GodotRTSCourse.Scripts.Game;

public partial class UnitAudio : AudioStreamPlayer
{
    private AudioStream damageSound = GD.Load<AudioStream>(AudioPathConsts.DamageSound);
    private Unit unit;

    public override void _Ready()
    {
        this.unit = this.GetParent<Unit>();
        this.unit.TakeDamage += this.PlayDamageSound;
    }

    private void PlayDamageSound(int health)
    {
        this.PlaySound(this.damageSound);
    }

    private void PlaySound(AudioStream audio)
    {
        this.Stream = audio;
        this.Play();
    }

    public override void _ExitTree()
    {
        this.unit.TakeDamage -= this.PlayDamageSound;
    }
}
