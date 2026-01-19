using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.Objects;

internal class PlantData
{
	public string Path { get; set; }
	public float GrowthSpeed { get; set; }


	public PlantData(string path, float growthSpeed)
	{
		this.Path = path;
		this.GrowthSpeed = growthSpeed;
	}
}
