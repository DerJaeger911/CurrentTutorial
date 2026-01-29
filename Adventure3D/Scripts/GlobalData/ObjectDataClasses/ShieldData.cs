namespace Adventuregame.Scripts.GlobalData.ObjectDataClasses;

public partial class ShieldData : ItemDataBase
{
    public ShieldData()
    {
        this.Type = "shield";
    }
    public float Defense { get; set; }
}
