using Adventuregame.Scripts.GlobalData.Enums;

namespace Adventuregame.Scripts.GlobalData.ObjectDataClasses;

public partial class ShieldData : ItemDataBase
{
    public ShieldData()
    {
        this.Type = ItemTypeEnum.Shield;
    }
    public float Defense { get; set; }
}
