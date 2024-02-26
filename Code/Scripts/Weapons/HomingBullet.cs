using System;
using Godot;

namespace Weapons;

public partial class HomingBullet : Bullet
{

    [Export] public int HomingTicks { get; set; } //delay in ticks before the bullet targets the player location
    [Export] public float MaxRotationDeg { get; set; } //in degrees

    private Vector2 _target;
    public Node2D TargetNode { get; set; }



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
        //If the target is null, then abort launching bullet
        if (TargetNode == null || TargetNode.IsQueuedForDeletion())
        {
            GD.Print("WARNING: Homing Bullet target is not set, destroy bullet.");
            QueueFree();
            return;
        }
        
        _target = TargetNode.GlobalPosition;
        if (_ticksPassed == -1)
        {
            _targetDirection = (_target - GlobalPosition).Normalized(); //init bullet with direct direction

        }
        else if (_ticksPassed >= HomingTicks)
        {
            Vector2 directLine = (_target - GlobalPosition).Normalized();
            Vector2 oldDirection = _targetDirection;
            float rawAngle = directLine.Angle() - oldDirection.Angle(); //angle of new direction to old direction
            float maxAngleRad = MaxRotationDeg * MathF.PI / 180F; //max rotation in radians for later calculation

            float angle = BulletMath.NormalizeAngle(rawAngle);
            float homingAngle = BulletMath.RestrictHomingAngle(angle, maxAngleRad);
            _targetDirection = new Vector2(1, 0).Rotated(oldDirection.Angle() + homingAngle).Normalized();

            _ticksPassed = 0;

        }

        MoveProjectile();

        _ticksPassed++;
    }

    private void MoveProjectile()
    {
        Rotation = _targetDirection.Angle();

        MoveAndCollide(_targetDirection * Speed);
    }
}