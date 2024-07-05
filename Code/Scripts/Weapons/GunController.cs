using Godot;
using System;
using Code.Scripts.Audio;
using Code.Scripts.Enemies;
using Code.Scripts.Managers;
using Code.Scripts.Movement;
using Code.Scripts.UI.Shop;
using Godot.Collections;

namespace Code.Scripts.Weapons;

/// <summary>
/// Controls the gun of the player and enemies, handling the shooting of bullets, rotation and weapon stats.
/// </summary>
public partial class GunController : Node2D
{
	[Export] public WeaponStats WeaponStats { get; set; }

	public float TimeBetweenShots { get; private set; }
	protected MusicController MusicController;
	private bool _canShoot = true;
	private AnimatedSprite2D _sprite;
	private Timer _shootTimer;

	// Das ist sowas von dreckig, aber ist mir egal :O
	private bool _isPlayerGunController;

	public override void _Ready()
	{
		MusicController = GetNode<MusicController>("/root/MusicController");
		_sprite = GetNode<AnimatedSprite2D>("GunSprite");
		_shootTimer = GetNode<Timer>("ShootTimer");
		_shootTimer.Timeout += () => _canShoot = true;

		TimeBetweenShots = 1 / WeaponStats.BulletsPerSecond;


		if (GetParent().IsInGroup("Player"))
		{
			SetWeapon();
			_isPlayerGunController = true;
		}

		ShopManager.Instance.OnWeaponEquipped += SetWeapon;
	}

	public override void _ExitTree()
	{
		ShopManager.Instance.OnWeaponEquipped -= SetWeapon;
	}

	/// <summary>
	/// Sets the weapon stats of the gun controller to the currently equipped weapon.
	/// </summary>
	public void SetWeapon()
	{
		if (GetParent().IsInGroup("Player"))
		{
			Weapon weapon = ShopManager.Instance.GetEquippedWeapon();
			WeaponStats = weapon.WeaponStats;
			TimeBetweenShots = 1 / weapon.WeaponStats.BulletsPerSecond;
		}
	}

	/// <summary>
	/// Rotates the gun towards the target.
	/// </summary>
	/// <param name="target"></param>
	public void RotateTowards(Vector2 target)
	{
		LookAt(target);
	}

	/// <summary>
	/// Shoots a bullet from the gun, if the gun is ready to shoot.
	/// </summary>
	public void Shoot()
	{
		if (_canShoot)
		{
			SpawnBullet();

			//Play Sound
			MusicController.Play(Sound.TankShooting);

			//reset the time until fire
			_canShoot = false;
			_shootTimer.Start(TimeBetweenShots);

			//Play animation()
			_sprite.Play("Shoot");
			_sprite.Frame = 0;
		}

	}

	/// <summary>
	/// Spawns the bullet at the muzzle of the gun, setting its damage, speed and target.
	/// </summary>
	private void SpawnBullet()
	{
		//create a bullet and set its damage and speed
		Bullet bullet = WeaponStats.BulletScene.Instantiate<Bullet>();
		bullet.Damage = WeaponStats.Damage;
		if (_isPlayerGunController)
		{
			bullet.Damage = bullet.Damage * PlayerManager.Instance.PlayerStats.CurrentDamageModifier;
		}

		//if it is a homing bullet shot by the player then set the target to a random enemy.
		if (bullet is HomingBullet hb)
			_setHomingBulletTarget(hb);

		// set rotation and velocity of bullet
		bullet.Rotation = GlobalRotation;
		bullet.LinearVelocity = bullet.Transform.X.Normalized() * WeaponStats.BulletSpeed;

		// set position of bullet to muzzle
		var muzzle = GetNode<Marker2D>("muzzle");
		bullet.GlobalPosition = muzzle.GlobalPosition;
		bullet.Shooter = GetParent() as Node2D;
		bullet.Speed = WeaponStats.BulletSpeed;
		bullet.AddToGroup("Bullets");

		//add the bullet to the scene tree 
		GetTree().GetFirstNodeInGroup("Level").AddChild(bullet);
	}

	/// <summary>
	/// Sets the target of the homing bullet to the player, if the owner is an enemy, or to the closest enemy of the mouse, if the owner is the player.
	/// </summary>
	/// <param name="bullet"></param>
	/// <exception cref="ArgumentException"></exception>
	private void _setHomingBulletTarget(HomingBullet bullet)
	{
		Node parent = GetParent();
		if (parent is PlayerMovement)
		{
			var enemies = GetTree().GetNodesInGroup("Enemy");
			if (enemies.Count > 0)
			{
				// select enemy which is closest to mouse position
				bullet.TargetNode = _getClosestEnemy(enemies);
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


	/// <summary>
	/// Returns the closest enemy to the mouse position.
	/// </summary>
	/// <param name="enemies"></param>
	/// <returns></returns>
	private Node2D _getClosestEnemy(Array<Node> enemies)
	{
		// select enemy which is closest to mouse position
		Node2D closestEnemy = null;
		float minDistance = float.MaxValue;
		foreach (Node enemy in enemies)
		{
			if (enemy is Node2D enemy2d)
			{
				float distance = enemy2d.GlobalPosition.DistanceTo(GetGlobalMousePosition());
				if (distance < minDistance)
				{
					minDistance = distance;
					closestEnemy = enemy2d;
				}
			}
		}

		return closestEnemy;
	}
}

