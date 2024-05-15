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
    protected MusicController musicController;
    private bool _canShoot = true;
    private AnimatedSprite2D _sprite;
    private Timer _shootTimer;

    // Das ist sowas von dreckig, aber ist mir egal :O
    private bool isPlayerGunController = false;

    public override void _Ready()
    {
        musicController = GetNode<MusicController>("/root/MusicController");
        _sprite = GetNode<AnimatedSprite2D>("GunSprite");
        _shootTimer = GetNode<Timer>("ShootTimer");
        _shootTimer.Timeout += () => _canShoot = true;

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
        if (_canShoot)
        {
            SpawnBullet();

            //Play Sound
            musicController.Play(Sound.TankShooting);

            //reset the time until fire
            _canShoot = false;
            _shootTimer.Start(timeBetweenShots);

            //Play animation()
            _sprite.Play("Shoot");
            _sprite.Frame = 0;
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
        bullet.Speed = weaponStats.bulletSpeed;
        bullet.AddToGroup("Bullets");

        //add the bullet to the scene tree 
        GetTree().GetFirstNodeInGroup("Level").AddChild(bullet);
    }

    private void _setHomingBulletTarget(HomingBullet bullet)
    {
        Node parent = GetParent();
        if (parent is PlayerMovement)
        {
            var enemies = GetTree().GetNodesInGroup("Enemy");
            if (enemies.Count > 0)
            {
                // select enemy which is closest to mouse position
                Node2D closestEnemy = null;
                float minDistance = float.MaxValue;
                foreach (Node2D enemy in enemies)
                {
                    float distance = enemy.GlobalPosition.DistanceTo(GetGlobalMousePosition());
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestEnemy = enemy;
                    }
                }

                bullet.TargetNode = closestEnemy;
            }
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

