using System;
using Components;
using Godot;

namespace Weapons;

public partial class GrenadeBullet : Bullet
{

    [Export] public float ArcAngleOffsetDeg { get; set; }
    [Export] public float GravitationalForce { get; set; }
    [Export] public float DamageRadius { get; set; }
    

    //when checking if a grenade hit the ground, we want to compare two normalized vectors to check if they are collinear with tolerance
    private readonly double _absoluteCollinearTollerance = 1e-2;
    private Vector2 _gravity;
    private Vector2 _shootDirection;
    private Vector2 _trajectoryDirection;
    
    private double _time = 10e-6; //time after the grenade was shot
    private readonly float _timeDiff = 0.01F; //time passed between to frames for the grenade
    private float _shootXDirectionSign;
    private float _shootYDirectionSign;
    private Vector2 _originalGravity;
    private Vector2 _referenceAxis;

    protected override void Setup()
    {
        _shootDirection = new Vector2(1, 0).Rotated(GlobalRotation).Normalized();
        float angleToXAxis = _shootDirection.Angle();
        _shootXDirectionSign = _shootDirection.X < 0 ? -1 : 1;
        _shootYDirectionSign = _shootDirection.Y < 0 ? -1 : 1;
        
        //add arch-offset to simulate a 3d-trajectory in a 2d plane
        float offsetAngle = GetArcAngleOffset(angleToXAxis);
        float normalizedAngle = NormalizeAngle(angleToXAxis + offsetAngle);
        _trajectoryDirection = new Vector2(1,0).Rotated(normalizedAngle).Normalized() * Speed;
        
        //get a vector perpendicular to the shoot-direction to act as a gravitational vector for the grenade trajectory
        //that is pointing to the x-Axis
        _gravity = GetSimulatedGravityVector(_shootDirection);
        _originalGravity = _gravity;
    }

    protected override void Move()
    {
        Rotation = LinearVelocity.Angle();
        LinearVelocity = (_trajectoryDirection + _gravity).Normalized() * Speed;
        
        //update gravity vector according to "Schiefer Wurf"
        _gravity.X = _originalGravity.X + 0.5F * GravitationalForce * Mathf.Pow((float)_time, 2) * _shootXDirectionSign;
        _gravity.Y = _originalGravity.Y + 0.5F * GravitationalForce * Mathf.Pow((float)_time, 2) * -_shootYDirectionSign;
        _time += _timeDiff;
        
        MoveAndCollide(LinearVelocity);
        
        //collision detection is triggered iff the current bullet position is collinear to the original shoot direction
        Vector2 pos = (GlobalPosition - player.GlobalPosition);
        //if the distance is too small, then the collision was triggered right after the bullet was shot, so ignore it
        
        if (pos.Length() <= 100) //player size 78x66
        {
            return; //ignore collision for close bullets
        }
        
        if (InSymmetricInterval((float) _absoluteCollinearTollerance, pos.Normalized().X - _shootDirection.X) &&
            InSymmetricInterval((float) _absoluteCollinearTollerance, pos.Normalized().Y - _shootDirection.Y))
        {
            //since the bullet is relatively close to its ground hit-point, trigger the collision
            OnExplode();
        }
    }

    public void OnExplode()
    {
        Vector2 hitPosition = GlobalPosition;
        //check for all damageable objects inside the explosion radius and damage them according to their distance to the explosion source
        foreach (Node node in GetTree().GetNodesInGroup("Damageable"))
        {
            if (node is CharacterBody2D body)
            {
                float distance = (body.GlobalPosition - hitPosition).Length();
                if (distance <= DamageRadius)
                {
                    //body in hit range, so damage it according to dmg = bulletDamage / radius
                    float finalDamage = damage / distance;
                    node.GetNode<HealthComponent>("HealthComponent").TakeDamage(finalDamage);
                }
            }
        }
        
        particles.EmitParticles(this, Scene.Explosion);
        musicController.Play(Sound.RocketExplosion);
        Destroy();
    }

    /**
     * Check if the given toCheck float is inside the interval [-interval;interval].
     */
    private bool InSymmetricInterval(float interval, float toCheck)
    {
        return toCheck >= -1.0 * interval && toCheck <= interval;
    }
    
    private float NormalizeAngle(float angle)
    {
        if (angle > Mathf.Pi)
        {
            return angle - 2 * Mathf.Pi;
        }

        if (angle < -Mathf.Pi)
        {
            return 2 * Mathf.Pi - angle;
        }

        return angle;
    }

    /**
     * Get a vector perpendicular to the shoot-direction to act as a gravitational vector for the grenade trajectory
     * that is oriented to the x-Axis. Note that this vector is normalized.
     */
    private Vector2 GetSimulatedGravityVector(Vector2 direction)
    {
        //check if the angle is closer to the y or x axis. If closer to x, rotate by +pi/2 angle, if closer to y, rotete by -pi/2
        float angle = direction.Angle();
        float axisSign = InSymmetricInterval(Mathf.Pi / 4, angle) || angle > 3*Mathf.Pi / 4 || angle < -3*Mathf.Pi / 4 ? 1 : -1;
        _referenceAxis = axisSign > 0 ? new Vector2(1, 0) : new Vector2(0, 1);
        
        return direction.Rotated(axisSign * MathF.PI / 2);
    }

    /**
     * Returns the offset angle of the grenade trajectory to simulate a parabola in 3d on a 2d grid.
     */
    private float GetArcAngleOffset(float angle)
    {
        if (Mathf.Sign(angle) > 0) //below x-Axis
        {
            if (angle < Mathf.Pi / 2.0)
            {
                //if angle larger than 90° then remove offset angle to simulate a grenade flying "upwards"
                return 1 * Mathf.DegToRad(ArcAngleOffsetDeg);
            }
            return -1 * Mathf.DegToRad(ArcAngleOffsetDeg);
        }
       
        //same for negative angles but signs flipped
        if (angle > -Mathf.Pi / 2.0)
        {
            return -1 * Mathf.DegToRad(ArcAngleOffsetDeg);
        }
        
        return +1 * Mathf.DegToRad(ArcAngleOffsetDeg);
    }
}