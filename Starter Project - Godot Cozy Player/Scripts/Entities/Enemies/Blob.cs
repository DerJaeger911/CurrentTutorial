namespace Dcozysandbox.Scripts.Enemies;

public partial class Blob : Enemy
{
    protected override int MaxHealth { get; set; } = 3;

    public override float Speed { get; set; } = 30;



	public override void _Ready()
    {

		base._Ready();
    }

    protected override void SetAnimation()
    {
        base.SetAnimation();


    }
}
