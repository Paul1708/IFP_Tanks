using System;
using Godot;

namespace Weapons;

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
        _laserLine.Points[1] = Vector2.Zero;
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
            _targetPoint = ToLocal(GetCollisionPoint());
            _laserParticles.GlobalRotation = GetCollisionNormal().Angle();
            _laserParticles.Position = _targetPoint;
            _laserLine.Points[1] = _targetPoint;

            _laserBeamParticles.EmissionRectExtents = _targetPoint * 0.5F;
            _laserBeamParticles.Position = _targetPoint * 0.5F;
            
            QueueRedraw();
            
        }
        
        _laserLine.Points[0] = _gun.GlobalPosition;
    }

    //draw laser line
    public override void _Draw()
    {
        DrawLine(_laserLine.Points[0], _targetPoint, _laserLine.DefaultColor, 10);
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        _laserParticles.Emitting = _isActive;
        _laserBeamParticles.Emitting = _isActive;
        _laserHitParticles.Emitting = _isActive;
        
        SetPhysicsProcess(_isActive);
    }
}