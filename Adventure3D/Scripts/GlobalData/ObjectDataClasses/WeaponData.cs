using Adventuregame.Scripts.GlobalData.Enums;
using Godot;

namespace Adventuregame.Scripts.GlobalData.ObjectDataClasses;

public partial class WeaponData : ItemDataBase
{
    public WeaponData()
    {
        this.Type = ItemTypeEnum.Weapon;
    }
    public int Damage { get; set; }
	public string Animation { get; set; }
	public float Range { get; set; }
	public AudioStream Audio { get; set; }
}
