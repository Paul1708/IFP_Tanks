namespace Enemies;

public partial class BasicBehaviour : Behaviour
{
	public override void Setup()
	{
		Navigation.NavigateTowards(Player.GlobalPosition);
	}
	public override void ExecuteBehaivour()
	{
		Gun.RotateTowards(Player.GlobalPosition);
		Gun.Shoot();
		Navigation.NavigateTowards(Player.GlobalPosition);
	}
}
