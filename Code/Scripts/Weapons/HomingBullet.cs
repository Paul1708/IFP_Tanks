using System;
using Godot;

namespace Weapons;

public partial class HomingBullet : Bullet
{

    [Export] public int HomingTicks { get; set; } //delay in ticks before the bullet targets the player location
    [Export] public float MaxRotationDeg { get; set; } //in degrees

    private Vector2 Target { get; set; }



    private Vector2 _targetDirection;
    private int _ticksPassed = -1;

    public override void Destroy()
    {
        MusicController.Play(Sound.RocketExplosion);
        Particles.EmitParticles(this, Scene.Explosion);
        QueueFree();
    }

    protected override void Move()
    {
        Target = Player.GlobalPosition;
        if (_ticksPassed == -1)
        {
            _targetDirection = (Target - GlobalPosition).Normalized(); //init bullet with direct direction

        }
        else if (_ticksPassed >= HomingTicks)
        {
            Vector2 directLine = (Target - GlobalPosition).Normalized();
            Vector2 oldDirection = _targetDirection;
            float rawAngle = directLine.Angle() - oldDirection.Angle(); //angle of new direction to old direction
            float maxAngleRad = MaxRotationDeg * MathF.PI / 180F; //max rotation in radians for later calculation

            float angle = NormalizeAngle(rawAngle);
            float homingAngle = RestrictHomingAngle(angle, maxAngleRad);
            _targetDirection = new Vector2(1, 0).Rotated(oldDirection.Angle() + homingAngle).Normalized();

            _ticksPassed = 0;

        }

        MoveProjectile();

        _ticksPassed++;
    }

    /**
     * Check if a phase shift occurred in the angle, i.e. the angle exceeds PI in positive or negative direction.
     * If it occured, then the angle value will be changed to the according angle ignoring the phase shift.
     */
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
     * Ensure that the angle does not exceed the interval [-maxAngleRadians, maxAngleRadians]. If it is not inside
     * this interval, the closed interval border will be returned.
     */
    private float RestrictHomingAngle(float angle, float maxAngleRadians)
    {
        if (Math.Sign(angle) < 0)
        {
            return Math.Max(angle, -maxAngleRadians);
        }
        else
        {
            return Math.Min(angle, maxAngleRadians);
        }
    }

    private void MoveProjectile()
    {
        Rotation = _targetDirection.Angle();

        MoveAndCollide(_targetDirection * Speed);
    }
}