using Godot;
using System;

namespace Weapons;

public partial class GunController : Node2D
{
    [Export] PackedScene bulletScene;
    [Export] public float bulletSpeed { get; set; } //speed of the bullet
    [Export] public float bulletsPerSecond { get; set; } //how many bullets per second
    [Export] public float bulletDamage { get; set; } //how much damage the bullet does


    public float timeBetweenShots { get; private set; }

    private float timeUntilNextShot = 0f; //how much time until the gun can fire again
    private bool canShoot = true;

    public override void _Ready()
    {
        if (bulletsPerSecond <= 0)
        {
            throw new Exception("Bullets per second must be greater than 0");
        }
        timeBetweenShots = 1 / bulletsPerSecond; //calculate the fire rate
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
            RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>(); //create a bullet

            // set rotation and velocity of bullet
            bullet.Rotation = GlobalRotation;
            bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;

            // set position of bullet to muzzle
            var muzzle = GetNode<Marker2D>("muzzle");
            bullet.GlobalPosition = muzzle.GlobalPosition;

            GetTree().Root.AddChild(bullet); //add the bullet to the scene tree 

            timeUntilNextShot = timeBetweenShots; //reset the time until fire
            canShoot = false;
        }

    }
}

