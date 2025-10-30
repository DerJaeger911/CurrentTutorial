using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMIapp.Scripts;

public static class Helpers
{
    public static decimal DecimalSnapped(decimal value, decimal step)
    {
        return Math.Round(value / step) * step;
    }
}
