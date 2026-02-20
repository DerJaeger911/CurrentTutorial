using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twentyfourtyeight.src.main.SeedGeneration;

//Init in Autoload File with _ = SeedSettings.Instance
public class SeedSettings
{
    private static SeedSettings instance;

    public static SeedSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SeedSettings();
            }
            return instance;
        }
    }


    public int ResourceSeed { get; private set; }
    public int TerrainSeed { get; private set; }
    public int MaxIce {  get; private set; }
    public bool UseRandomizedResourceSeed { get; private set; }
    public bool UseRandomizedTerrainSeed { get; private set; }
    public bool UseRandomizedIceSeed { get; private set; }
    public bool UseOnlyTerrainsSeed { get; private set; }

    public void SetSeedSettings(int resourceSeed, int terrainSeed, int maxIce, bool useRandomizeResourceSeed, bool useRandomizeTerrainSeed, bool useRandomizedIceSeed, bool useOnlyTerrainsSeed)
    {
        this.ResourceSeed = resourceSeed;
        this.TerrainSeed = terrainSeed;
        this.MaxIce = maxIce;
        this.UseRandomizedResourceSeed = useRandomizeResourceSeed;
        this.UseRandomizedTerrainSeed = useRandomizeTerrainSeed;
        this.UseRandomizedIceSeed = useRandomizedIceSeed;
        this.UseOnlyTerrainsSeed = useOnlyTerrainsSeed;
    }
}
