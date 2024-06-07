using Code.Scripts.Managers;
using Godot;

namespace Code.Scripts.Weapons
{
    public partial class BasicBullet : Bullet
    {
        protected override Node Move()
        {
            KinematicCollision2D collided = MoveAndCollide(LinearVelocity);
            if (collided != null)
            {
                var collider = collided.GetCollider();
                if (collider is Node)
                {
                    return collider as Node;
                }
            }

            return null;
        }

        public override void Destroy()
        {
            Particles.EmitParticles(this, Scene.BulletCrack);
            QueueFree();
        }

        protected override void OnOtherHit()
        {
            Destroy();
        }
    }

}


