using Godot;
using System;
using Player;
using Managers;
using Shop;

namespace Weapons;

public partial class GunController : Node2D
{
    [Export] public WeaponStats weaponStats { get; set; }

    public float timeBetweenShots { get; private set; }
    private bool canShoot = true;
    private AnimatedSprite2D sprite;
    private Timer shootTimer;
    protected MusicController musicController;

    // Das ist sowas von dreckig, aber ist mir egal :O
    private bool isPlayerGunController = false;

    public override void _Ready()
    {
        musicController = GetNode<MusicController>("/root/MusicController");
        sprite = GetNode<AnimatedSprite2D>("GunSprite");
        shootTimer = GetNode<Timer>("ShootTimer");
        shootTimer.Timeout += () => canShoot = true;

        timeBetweenShots = 1 / weaponStats.bulletsPerSecond;


        if (GetParent().IsInGroup("Player"))
        {
            SetWeapon();
            isPlayerGunController = true;
        }

        ShopManager.Instance.OnWeaponEquipped += SetWeapon;
    }

    public override void _ExitTree()
    {
        ShopManager.Instance.OnWeaponEquipped -= SetWeapon;
    }

    public void SetWeapon()
    {
        if (GetParent().IsInGroup("Player"))
        {
            Weapon weapon = ShopManager.Instance.GetEquippedWeapon();
            weaponStats = weapon.weaponStats;
            timeBetweenShots = 1 / weapon.weaponStats.bulletsPerSecond;
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
        //create a bullet and set its damage and speed
        Bullet bullet = weaponStats.bulletScene.Instantiate<Bullet>();
        bullet.damage = weaponStats.damage;
        if (isPlayerGunController)
        {
            bullet.damage = (int)(bullet.damage * PlayerManager.Instance.PlayerStats.CurrentDamageModifier);
        }

        //if it is a homing bullet shot by the player then set the target to a random enemy.
        if (bullet is HomingBullet hb)
            _setHomingBulletTarget(hb);

        // set rotation and velocity of bullet
        bullet.Rotation = GlobalRotation;
        bullet.LinearVelocity = bullet.Transform.X.Normalized() * weaponStats.bulletSpeed;

        // set position of bullet to muzzle
        var muzzle = GetNode<Marker2D>("muzzle");
        bullet.GlobalPosition = muzzle.GlobalPosition;
        bullet.Shooter = GetParent() as Node2D;
        bullet.AddToGroup("Bullets");
        
        //add the bullet to the scene tree 
        GetTree().GetFirstNodeInGroup("Level").AddChild(bullet);
    }

    private void _setHomingBulletTarget(HomingBullet bullet)
    {
        Node parent = GetParent();
        if (parent is PlayerMovement)
        {
            if (GetTree().GetNodesInGroup("Enemy").Count > 0)
                bullet.TargetNode = GetTree().GetNodesInGroup("Enemy").PickRandom() as Node2D;
        }
        else if (parent is Enemy)
        {
            bullet.TargetNode = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        }
        else
        {
            throw new ArgumentException("Invalid bullet owner");
        }
    }

}

