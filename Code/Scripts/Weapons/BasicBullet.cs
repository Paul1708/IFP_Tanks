namespace Weapons
{
    public partial class BasicBullet : Bullet
    {
        protected override void Move()
        {
            MoveAndCollide(LinearVelocity);
        }

        public override void Destroy()
        {
            Particles.EmitParticles(this, Scene.BulletCrack);
            QueueFree();
        }
        
    }

}


