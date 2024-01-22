using Godot;

namespace Weapons;

public partial class BouncingBullet : Bullet
{
    [Export]
    public int MaxBounces { get; set; }
    private int _currentBounces;
    private Timer BounceTimer;
    private bool canBounce = true;

    protected override void Setup()
    {
        // Bullet only can bounce every 0.1 seconds
        BounceTimer = GetNode<Timer>("BounceTimer");
        BounceTimer.Timeout += () => canBounce = true;
    }
    protected override void Move()
    {

        var result = MoveAndCollide(LinearVelocity);
        if (result != null)
        {
            NormalCollisionVector = result.GetNormal().Normalized();

            if (result.GetCollider() is TileMap)
            {
                if (canBounce) BounceOfWall();
            }
        }
    }

    private void BounceOfWall()
    {
        LinearVelocity = LinearVelocity.Bounce(NormalCollisionVector);
        Rotation = LinearVelocity.Angle();

        _currentBounces++;
        BounceTimer.Start();
        canBounce = false;

        if (_currentBounces > MaxBounces)
        {
            ParticleController particles = GetNode<ParticleController>("/root/ParticleController");
            particles.EmitParticles(this, Scene.BulletCrack);
            Destroy();
        }
    }

    protected override void OnAnythingHit()
    {
        //Do nothing, so the bullet does not get destroyed
    }

    protected override void OnPlayerHit()
    {
        Destroy();
    }
    protected override void OnEnemyHit()
    {
        Destroy();
    }
    protected override void OnOtherHit()
    {
        Destroy();
    }

}