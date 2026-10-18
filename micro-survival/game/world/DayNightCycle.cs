using Godot;
using System;

public partial class DayNightCycle : Node3D
{
    [ExportGroup("Slots")]
    [Export]
    private DirectionalLight3D sun;
    [Export]
    private DirectionalLight3D moon;
    [Export]
    private WorldEnvironment sky;
    [ExportGroup("Time")]
    [Export]
    private float dayLength = 20f;
    [Export(PropertyHint.Range, "0,24,0.1")]
    private float startTime = 0.0f;
    [ExportGroup("Sun")]
    [Export]
    private Gradient sunColor;
    [Export]
    private Curve sunIntensity;
    [ExportGroup("Moon")]
    [Export]
    private Gradient moonColor;
    [Export]
    private Curve moonIntensity;
    [ExportGroup("Sky")]
    [Export]
    private Gradient skyTopColor;
    [Export]
    private Gradient skyHorizonColor;

    private float time;
    private float timeRate;

    public override void _Ready()
    {
        this.timeRate = 1.0f / this.dayLength;
        this.time = this.startTime / 24.0f;
    }

    public override void _Process(double delta)
    {
        this.SunRotation(delta);
        this.MoonRotation(delta);
        this.SkyColor(delta);
    }

    private void SunRotation(double delta)
    {
        this.time += this.timeRate * (float)delta;

        if (this.time >= 1f)
        {
            this.time = 0f;
        }

        float sunRotX = this.time * 360f + 90f;

        this.sun.RotationDegrees = new Vector3(
            sunRotX,
            this.sun.RotationDegrees.Y,
            this.sun.RotationDegrees.Z
        );

        this.sun.LightColor = this.sunColor.Sample(this.time);
        this.sun.LightEnergy = this.sunIntensity.Sample(this.time);

        this.sun.Visible = this.sun.LightEnergy > 0;
    }

    private void MoonRotation(double delta)
    {
        float moonRotX = this.time * 360f + 270f;
        this.moon.LightColor = this.moonColor.Sample(this.time);
        this.moon.LightEnergy = this.moonIntensity.Sample(this.time);

        this.moon.Visible = this.moon.LightEnergy > 0;
    }

    private void SkyColor(double delta)
    {
        this.sky.Environment.Sky.SkyMaterial.Set("sky_top_color", this.skyTopColor.Sample(this.time));
        this.sky.Environment.Sky.SkyMaterial.Set("sky_horizon_color", this.skyHorizonColor.Sample(this.time));
        this.sky.Environment.Sky.SkyMaterial.Set("ground_bottom_color", this.skyTopColor.Sample(this.time));
        this.sky.Environment.Sky.SkyMaterial.Set("ground_horizon_color", this.skyHorizonColor.Sample(this.time));
    }
}
