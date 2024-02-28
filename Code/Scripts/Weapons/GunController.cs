using Godot;
using System;
using Managers;
using Shop;

namespace Weapons;

public partial class GunController : Node2D
{
    [Export] PackedScene bulletScene;
    [Export] public float bulletsPerSecond { get; set; } //how many bullets per second
    [Export] public int bulletDamage { get; set; } //how much damage each bullet does


    public float timeBetweenShots { get; private set; }

    private bool canShoot = true;

    private AnimatedSprite2D sprite;
    private Timer shootTimer;

    protected MusicController musicController;

    public override void _Ready()
    {
        musicController = GetNode<MusicController>("/root/MusicController");

        if (bulletsPerSecond <= 0)
        {
            throw new Exception("Bullets per second must be greater than 0");
        }
        timeBetweenShots = 1 / bulletsPerSecond; //calculate the fire rate

        sprite = GetNode<AnimatedSprite2D>("GunSprite");
        shootTimer = GetNode<Timer>("ShootTimer");

        shootTimer.Timeout += () => canShoot = true;
        
        if (GetParent().IsInGroup("Player")) SetWeapon();
    }

    public void SetWeapon () 
    {
        Weapon weapon = ShopManager.Instance.GetEquippedWeapon();
        bulletScene = weapon.bulletScene;
    }

    public void RotateTowards(Vector2 target)
    {
        LookAt(target);
    }

    public void Shoot()
    {
        if (canShoot)
        {

            SpawnBullet();

            //Play Sound
            musicController.Play(Sound.TankShooting);

            //reset the time until fire
            canShoot = false;
            shootTimer.Start(timeBetweenShots);

            //Play animation()
            sprite.Play("Shoot");
            sprite.Frame = 0;
        }

    }

    private void SpawnBullet()
    {
        //create a bullet
        Bullet bullet = bulletScene.Instantiate<Bullet>();
        bullet.damage = bulletDamage;

        // set rotation and velocity of bullet
        bullet.Rotation = GlobalRotation;
        bullet.LinearVelocity = bullet.Transform.X.Normalized() * bullet.Speed;

        // set position of bullet to muzzle
        var muzzle = GetNode<Marker2D>("muzzle");
        bullet.GlobalPosition = muzzle.GlobalPosition;

        //add the bullet to the scene tree 
        GetTree().GetFirstNodeInGroup("Level").AddChild(bullet);
    }
}

