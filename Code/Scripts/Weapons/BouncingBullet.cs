using Components;
using Godot;

namespace Weapons;

public partial class BouncingBullet : Bullet
{
    [Export]
    public int MaxBounces { get; set; }
    private int _currentBounces;
    private Timer _bounceTimer;
    private bool _canBounce = true;

    protected override void Setup()
    {
        // Bullet only can bounce every 0.1 seconds
        _bounceTimer = GetNode<Timer>("BounceTimer");
        _bounceTimer.Timeout += () => _canBounce = true;
    }

    public override void Destroy()
    {
        particles.EmitParticles(this, Scene.BulletCrack);
        QueueFree();
    }

    protected override void Move()
    {

        var result = MoveAndCollide(LinearVelocity);
        if (result != null)
        {
            NormalCollisionVector = result.GetNormal().Normalized();

            if (result.GetCollider() is TileMap && _canBounce)
                BounceOfWall();
        }
    }

    private void BounceOfWall()
    {
        LinearVelocity = LinearVelocity.Bounce(NormalCollisionVector);
        Rotation = LinearVelocity.Angle();

        _currentBounces++;
        _bounceTimer.Start();
        _canBounce = false;

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

    public override void OnDamageableHit(Node node)
    {
        node.GetNode<HealthComponent>("HealthComponent").TakeDamage(damage);
        Destroy();
    }
    protected override void OnOtherHit()
    {
        Destroy();
    }

}