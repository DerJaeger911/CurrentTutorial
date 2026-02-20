using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace twentyfourtyeight.src.main.SeedGeneration;

public class SeedGenerator
{
    private readonly SeedSettings settings = SeedSettings.Instance;
    private readonly RandomNumberGenerator cellRng = new();
    private readonly Random rnd = new();

    private int currentTerrainSeed;
    private int currentResourceSeed;

    public SeedGenerator()
    {
        this.currentTerrainSeed = this.settings.UseRandomizedTerrainSeed ? (int)GD.Randi() : this.settings.TerrainSeed;

        if (this.settings.UseOnlyTerrainsSeed)
        {
            this.currentResourceSeed = this.currentTerrainSeed;
        }
        else
        {
            this.currentResourceSeed = this.settings.UseRandomizedResourceSeed ? (int)GD.Randi() : this.settings.ResourceSeed;
        }
    }


    public int GetResourceSeed(int x, int y, int min, int max)
    {
        ulong stableSeed = (ulong)(this.currentResourceSeed + (x * 31) + (y * 997));
        this.cellRng.Seed = stableSeed;

        int seed = this.settings.UseRandomizedResourceSeed ? this.rnd.Next(min, max) : this.cellRng.RandiRange(min, max);

        return seed;
    }

    public int GetTerrainSeed() => this.currentTerrainSeed;

    public int GetIceSeed(int x)
    {
        this.cellRng.Seed = (ulong)(this.currentTerrainSeed + (x * 13));

        int seed = this.settings.UseRandomizedIceSeed ? this.rnd.Next(this.settings.MaxIce) : this.cellRng.RandiRange(0, this.settings.MaxIce);
        return seed;
    }
}
