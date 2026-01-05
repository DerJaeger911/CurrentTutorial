using bullethellcourse.Scripts.Entities;
using Godot;

namespace bullethellcourse.Scripts.Items;

public partial class Potion : Item
{
	[Export]
	private PotionTypeEnum potionType;
	[Export]
	private double potionValue;

    protected override void AnimateItem()
    {
        double t = Time.GetUnixTimeFromSystem();
        double s = 1 + (Mathf.Sin(t * 10) * 0.1);

        this.Scale = new Vector2((float)s, (float)s);


    }

    protected override void ProcessItemEffects(Player player)
    {
        switch (this.potionType)
        {
            case PotionTypeEnum.Health:
                player.Heal((int)this.potionValue);
                break;
            case PotionTypeEnum.ShootSpeed:
                player.ShootRate *= this.potionValue;
                player.AdditionalBulletSpeed += 20;
                break;
            case PotionTypeEnum.MoveSpeed:
                player.MaxSpeed *= (int)this.potionValue;
                break;
        }
    }
}