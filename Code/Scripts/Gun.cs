using Godot;
using System;

public partial class Gun : Node2D
{
    [Export] PackedScene bulletScene;
    [Export] public float bulletSpeed { get; set; } //speed of the bullet
    [Export] public float bulletsPerSecond { get; set; } //how many bullets per second
    [Export] public float bulletDamage { get; set; } //how much damage the bullet does


    public float timeBetweenShots { get; private set; }

    private float timeUntilNextShot = 0f; //how much time until the gun can fire again

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
        LookAt(GetGlobalMousePosition()); // Look at the mouse

        if (Input.IsActionJustPressed("left_mouse") && timeUntilNextShot > timeBetweenShots) //if the left mouse button is pressed and the gun can fire
        {
            shoot(); //shoot
        }
        else
        {
            timeUntilNextShot += (float)delta; //add the time since the last frame to the time until fire
        }
    }

    private void shoot()
    {
        RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>(); //create a bullet

        bullet.Rotation = GlobalRotation; //set the rotation of the bullet to the rotation of the gun
        bullet.LinearVelocity = bullet.Transform.X * bulletSpeed; //set the velocity of the bullet 

        var muzzle = GetNode<Marker2D>("muzzle"); //get the muzzle node
        bullet.GlobalPosition = muzzle.GlobalPosition; //set the position of the bullet to the muzzle

        GetTree().Root.AddChild(bullet); //add the bullet to the scene tree 

        timeUntilNextShot = 0f; //reset the time until fire
    }
}

