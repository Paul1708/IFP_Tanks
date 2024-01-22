
using Godot;
namespace Weapons
{
    public partial class BasicBullet : Bullet
    {
        protected override void Move()
        {
            MoveAndCollide(LinearVelocity);
        }
        
        
    }

}


