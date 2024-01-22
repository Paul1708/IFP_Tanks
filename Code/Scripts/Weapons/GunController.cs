using Godot;
using System;

namespace Weapons;

public partial class GunController : Node2D
{
    [Export] PackedScene bulletScene;
    [Export] public float bulletsPerSecond { get; set; } //how many bullets per second


    public float timeBetweenShots { get; private set; }

    private float timeUntilNextShot = 0f; //how much time until the gun can fire again
    private bool canShoot = false;

    public override void _Ready()
    {
        if (bulletsPerSecond <= 0)
        {
            throw new Exception("Bullets per second must be greater than 0");
        }
        timeBetweenShots = 1 / bulletsPerSecond; //calculate the fire rate
        timeUntilNextShot = timeBetweenShots;
    }

    public override void _Process(double delta)
    {
        timeUntilNextShot -= (float)delta; //add the time since the last frame to the time until fire

        if (timeUntilNextShot <= 0)
        {
            canShoot = true;
        }
    }

    public void RotateTowards(Vector2 target)
    {
        LookAt(target);
    }

    public void Shoot()
    {
        if (canShoot)
        {
            //create a bullet
            Bullet bullet = bulletScene.Instantiate<Bullet>();

            // set rotation and velocity of bullet
            bullet.Rotation = GlobalRotation;
            bullet.LinearVelocity = bullet.Transform.X.Normalized() * bullet.BulletSpeed;

            // set position of bullet to muzzle
            var muzzle = GetNode<Marker2D>("muzzle");
            bullet.GlobalPosition = muzzle.GlobalPosition;

            GetTree().GetFirstNodeInGroup("Level").AddChild(bullet); //add the bullet to the scene tree 

            timeUntilNextShot = timeBetweenShots; //reset the time until fire
            canShoot = false;
        }

    }
}

