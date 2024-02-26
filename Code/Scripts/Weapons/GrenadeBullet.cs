using Components;
using Godot;

namespace Weapons;

public partial class GrenadeBullet : Bullet
{

    [Export] public float ArcAngleOffsetDeg { get; set; } //beta
    [Export] public float GravitationalForce { get; set; } //g
    [Export] public float DamageRadius { get; set; }
    
    private RigidBody2D _shadow;
    private CpuParticles2D _targetSprite;
    
    //Values for simulated parabola
    private float _time; //time after the grenade was shot
    private readonly float _timeDiff = 0.1F; //time passed between to frames for the grenade
    private Vector2 _shootDirection;
    private Vector2 _shootDirectionNormal;
    private float _offsetAngle;
    private Vector2 _shootPosition; //position where the grenades was shot from
    private float _grenadeGroundHitTime;
    private Vector2 _hitLocation;
    

    protected override void Setup()
    {
        _offsetAngle = Mathf.DegToRad(ArcAngleOffsetDeg);
        
        //TODO: Disable this line if you dont want to set the grenade target to where the player clicked
        Speed = 0.93F * Mathf.Sqrt((GravitationalForce * (GetGlobalMousePosition() - GlobalPosition).Length()) / Mathf.Sin(2.0F*_offsetAngle));
        
        _shadow = GetNode<RigidBody2D>("Shadow");
        _targetSprite = GetNode<CpuParticles2D>("TargetPoint");
        
        _shootDirection = new Vector2(1, 0).Rotated(GlobalRotation).Normalized() * Speed;

        _shootDirectionNormal = _shootDirection.Rotated(Mathf.Pi/2).Normalized();
        _shootPosition = GlobalPosition;
        
        
        _grenadeGroundHitTime = (2 * Speed * Mathf.Sin(_offsetAngle)) / GravitationalForce;

        _hitLocation = _grenadeGroundHitTime * new Vector2(_shootDirection[0], _shootDirection[1])
                                       + new Vector2(_shootPosition[0], _shootPosition[1]);
        _targetSprite.GlobalPosition = _hitLocation;
    }

    protected override void Move()
    {
        _shadow.GlobalPosition = _shadowTrajectory(_shootDirection, _time);
        GlobalPosition = _trajectory(_shootDirection, _time);
        _targetSprite.GlobalPosition = _hitLocation;

        if (_time >= _grenadeGroundHitTime)
        {
            OnExplode();
        }

        _time += _timeDiff;
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
                    float finalDamage = Damage / distance;
                    node.GetNode<HealthComponent>("HealthComponent").TakeDamage(finalDamage);
                }
            }
        }
        
        Particles.EmitParticles(this, Scene.Explosion);
        MusicController.Play(Sound.RocketExplosion);
        Destroy();
    }

    /**
     * Return the position of the grenade trajectory at time t with respect to the x-Axis (base case)
     */
    private Vector2 _normalizedTrajectory(float t)
    {
        //x(t) = ||s|| * t * cos(b) from "Schiefer Wurf"
        float x = Speed * t * Mathf.Cos(_offsetAngle);
        //y(t) = -g/2 * t^2 + ||s||*t*sin(b) from "Schiefer Wurf"
        float y = -GravitationalForce / 2.0F * Mathf.Pow(t, 2.0F) + Speed * t * Mathf.Sin(_offsetAngle);

        return new Vector2(x, y);
    }

    /**
     * Call by value since the original shoot direction has to be preserved.
     * Return the position of the 3d trajectory projected on the 2d grid of the grenade at the given time t.
     */
    private Vector2 _trajectory(Vector2 shootDirection, float t)
    {
        Vector2 normalizedTrajectoryPoint = _normalizedTrajectory(t);
        return (t * shootDirection + _shootDirectionNormal * normalizedTrajectoryPoint[1]) + _shootPosition;
    }

    /**
     * Call by value since the original shoot direction has to be preserved.
     * Return the position of the grenade shadow after the given time t.
     */
    private Vector2 _shadowTrajectory(Vector2 shootDirection, float t)
    {
        return (t * shootDirection) + _shootPosition;
    }
}