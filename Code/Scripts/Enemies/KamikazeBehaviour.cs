using Godot;

namespace Enemies;

public partial class KamikazeBehaviour : Behaviour
{
    
    
    //TODO: Bullets from enemies cannot hit other enemies
    //TODO: Laser only damages in Bullet-Hit rate not every frame
    //TODO: Merge Develop with current branch for updated Bullet stats
    //TODO: When two bullets of same group i.e. shot by enemy collide, destroy the weaker one, if they are the same type delete a random one
    
    public override void Setup()
    {
        Navigation.NavigateTowards(Player.GlobalPosition);
        //Navigation.PathDesiredDistance = 9;
        //Navigation.PathMaxDistance = 9;
    }

    public override void ExecuteBehaivour()
    {
        if (Navigation.IsTargetReached())
        {
            _explode();
            return;
        }
        
        Navigation.NavigateTowards(Player.GlobalPosition);
    }

    private void _explode()
    {
        
    }
}