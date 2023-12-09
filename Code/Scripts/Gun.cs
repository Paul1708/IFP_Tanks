using Godot;
using System;

public partial class Gun : Node2D
{
    [Export] PackedScene bullet_scn;
    [Export] float bullet_speed = 600f; //speed of the bullet
    [Export] float bps = 5f; //how many bullets per second
    [Export] float bullet_damage = 30f; //how much damage the bullet does


    float fire_rate;

    float time_until_fire = 0f; //how much time until the gun can fire again

    public override void _Ready()
    {
        fire_rate = 1 / bps; //calculate the fire rate
    }

    public override void _Process(double delta)
    {
        LookAt(GetGlobalMousePosition()); // Look at the mouse

        if (Input.IsActionJustPressed("left_mouse") && time_until_fire > fire_rate) //if the left mouse button is pressed and the gun can fire
        {
            shoot(); //shoot
        }
        else
        {
            time_until_fire += (float)delta; //add the time since the last frame to the time until fire
        }
    }

    private void shoot() {
        RigidBody2D bullet = bullet_scn.Instantiate<RigidBody2D>(); //create a bullet

        bullet.Rotation = GlobalRotation; //set the rotation of the bullet to the rotation of the gun
        bullet.LinearVelocity = bullet.Transform.X * bullet_speed; //set the velocity of the bullet 

        var muzzle = GetNode<Marker2D>("muzzle"); //get the muzzle node
        bullet.GlobalPosition = muzzle.GlobalPosition; //set the position of the bullet to the muzzle

        GetTree().Root.AddChild(bullet); //add the bullet to the scene tree 

        time_until_fire = 0f; //reset the time until fire
    }
}
