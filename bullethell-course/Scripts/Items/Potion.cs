using bullethellcourse.Scripts.Entities;
using Godot;

namespace bullethellcourse.Scripts.Items;

public partial class Potion : Item
{
	[Export]
	private PotionTypeEnum potionType;
	[Export]
	private double potionValue;



    protected override void ProcessItemEffects(Player player)
    {
        switch (this.potionType)
        {
            case PotionTypeEnum.Health:
                player.Heal((int)this.potionValue);
                break;
            case PotionTypeEnum.ShootSpeed:
                player.ShootRate *= this.potionValue;
                break;
            case PotionTypeEnum.MoveSpeed:
                player.MaxSpeed *= (int)this.potionValue;
                break;
        }
    }
}