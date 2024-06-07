using Godot;

namespace Code.Scripts.Weapons;

public partial class Laser : RayCast2D
{

    private bool _isActive;
    private Line2D _laserLine;
    private CpuParticles2D _laserParticles;
    private CpuParticles2D _laserHitParticles;
    private CpuParticles2D _laserBeamParticles;
    private LaserGun _gun;
    private Vector2 _targetPoint;

    public void Setup(LaserGun gun)
    {
        _gun = gun;
        _laserLine = GetNode<Line2D>("LaserLine");
        _laserLine.SetPointPosition(0, Vector2.Zero);
        _laserParticles = _laserLine.GetNode<CpuParticles2D>("ShootLaserParticles");
        _laserHitParticles = _laserLine.GetNode<CpuParticles2D>("HitParticles");
        _laserBeamParticles = _laserLine.GetNode<CpuParticles2D>("BeamParticles");
        
        SetActive(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        ForceRaycastUpdate();
        if (IsColliding())
        {
            //Node collided = GetColliderRid()
            _updateLaserHit();
            _laserParticles.GlobalRotation = GetCollisionNormal().Angle();
            _laserParticles.Position = _laserLine.Points[1];

            _laserBeamParticles.EmissionRectExtents = _laserLine.Points[1] * 0.5F;
            _laserBeamParticles.Position = _laserLine.Points[1] * 0.5F;

            Node2D collidedWith = GetCollider() as Node2D;
            if(collidedWith != null && collidedWith.IsInGroup("Damageable"))
                _gun.OnDamageableHit(collidedWith);
            
            else if (collidedWith is Bullet b)
                b.OnCollision(this);
            
        }
        
        _laserLine.Points[0] = _gun.GlobalPosition;
    }
    

    public void SetActive(bool active)
    {
        _isActive = active;
        _laserParticles.Emitting = _isActive;
        _laserBeamParticles.Emitting = _isActive;
        _laserHitParticles.Emitting = _isActive;
        SetPhysicsProcess(_isActive);
    }

    /**
     * Rotate the drawn laser line along the given angle in radians.
     */
    public void MoveLaserRay(float globalRotationAngle)
    {
        GlobalRotation = globalRotationAngle;
        _updateLaserHit();
    }

    private void _updateLaserHit()
    {
        if (IsColliding())
        {
            _targetPoint = ToLocal(GetCollisionPoint());
            _laserLine.SetPointPosition(1, _targetPoint);
        }
            
        QueueRedraw();
    }
}