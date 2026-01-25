using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventuregame.Scripts.Global.ObjectDataClasses;

public partial class ShieldData : ItemDataBase
{
    public ShieldData()
    {
        this.Type = "shield";
    }
    public float Defense { get; set; }
}
