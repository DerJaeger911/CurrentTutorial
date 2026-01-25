using Adventuregame.Scripts.Global.ObjectDataClasses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventuregame.Scripts.Global.ObjectClasses;

public partial class WeaponData : ItemDataBase
{
    public WeaponData()
    {
        this.Type = "weapon";
    }
    public int Damage { get; set; }
	public string Animation { get; set; }
	public float Range { get; set; }
	public AudioStream Audio { get; set; }
}
