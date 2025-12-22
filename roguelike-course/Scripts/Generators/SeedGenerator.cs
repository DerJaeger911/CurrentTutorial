using System;

namespace RoguelikeCourse.Scripts.Generators;

internal class SeedGenerator
{
    private static readonly Lazy<SeedGenerator> _instance =
           new(() => new SeedGenerator());

    public static SeedGenerator Instance => _instance.Value;

    public int Seed { get; private set; }
    private Random random;

    private SeedGenerator()
    {
        this.Seed = -1;
        this.random = new Random();
    }

    public void Initialize(int seed)
    {
        if (seed == 0)
        {
            seed = Environment.TickCount ^ Guid.NewGuid().GetHashCode();
        }

        this.Seed = seed;
        this.random = new Random(seed);
    }

    public int NextInt(int min, int max)
        => this.random.Next(min, max);

    public double NextDouble()
        => this.random.NextDouble();
}
