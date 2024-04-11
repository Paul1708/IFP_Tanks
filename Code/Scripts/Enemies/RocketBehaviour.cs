namespace Enemies;

public partial class RocketBehaviour : BasicBehaviour
{

    public override void ExecuteBehaivour()
    {
        Gun.RotateTowards(Player.GlobalPosition);
        Gun.Shoot();
        //check if target reached, if so, find a new random target
        Navigation.NavigateTowards(TargetLocation);
        if ((Enemy.GlobalPosition - TargetLocation).Length() <= PathGoalHitRadius)
        {
            //find a new target
            TargetLocation = GetRandomTarget();
        }
    }
}