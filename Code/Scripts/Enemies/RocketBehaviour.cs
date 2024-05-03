using Godot;

namespace Enemies;

public partial class RocketBehaviour : BasicBehaviour
{
    private NavigationAgent2D NavAgent2Player { get; set; }
    private int _ticks = 100;

    public override void Setup()
    {
        NavAgent2Player = GetNode<NavigationAgent2D>("../NavAgent2Player");
        TargetLocation = GetRandomTarget();
    }
    public override void ExecuteBehaivour()
    {

        // when I remove this useless declaration, the navcomponent will not find a path
        // i really have no clue why so i will leave it in
        bool test = NavAgent2Player.IsTargetReachable();
        // only recalculate path every 20 ticks. Removing this will also break the pathfinding,
        // godot is so weird...
        if (_ticks >= 20)
        {
            NavAgent2Player.TargetPosition = Player.GlobalPosition;
            _ticks = 0;
        }
        _ticks++;

        // get the roation target, which is the 5th next point in the path. 
        //If the path is shorter than 7 points, rotate towards the player
        var gunTargetVector = NavAgent2Player.GetCurrentNavigationPath().Length > 4 ?
            NavAgent2Player.GetCurrentNavigationPath()[4] : Player.GlobalPosition;

        // Now lerp the gun rotation towards the target to avoid hard snapping
        var angle = (gunTargetVector - GlobalPosition).Angle();
        var lerpedAngle = Mathf.LerpAngle(Gun.GlobalRotation, angle, 0.025f);
        Gun.GlobalRotation = lerpedAngle;

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