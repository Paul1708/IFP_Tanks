
using Code.Scripts.Components;
using Code.Scripts.Managers;
using Code.Scripts.Movement;
using Godot;

namespace Code.Scripts.Weapons;

/// <summary>
/// Controls the bouncing bullet
/// </summary>
public partial class BouncingBullet : Bullet
{
	[Export]
	public int MaxBounces { get; set; }
	private int _currentBounces;
	private Timer _bounceTimer;
	private bool _canBounce = true;

	protected override void Setup()
	{
		// Bullet only can bounce every 0.1 seconds
		_bounceTimer = GetNode<Timer>("BounceTimer");
		_bounceTimer.Timeout += () => _canBounce = true;
	}

	/// <summary>
	/// Destroys the bullet and emits particles
	/// </summary>
	public override void Destroy()
	{
		Particles.EmitParticles(this, Scene.BulletCrack);
		QueueFree();
	}

	/// <summary>
	/// Moves the bullet forward. When it hits a wall, calls BounceOfWall
	/// </summary>
	/// <returns>The node that the bullet collided with</returns>
	protected override Node Move()
	{
		var result = MoveAndCollide(LinearVelocity);
		if (result != null)
		{
			NormalCollisionVector = result.GetNormal().Normalized();
			var collider = result.GetCollider();
			if (collider is Node)
			{
				if (collider is TileMap && _canBounce)
					BounceOfWall();

				return collider as Node;
			}
		}

		return null;
	}

	/// <summary>
	/// Bounces the bullet off the wall by calculating the new velocity and rotation
	/// </summary>
	private void BounceOfWall()
	{
		LinearVelocity = LinearVelocity.Bounce(NormalCollisionVector);
		Rotation = LinearVelocity.Angle();

		_currentBounces++;
		_bounceTimer.Start();
		_canBounce = false;

		if (_currentBounces > MaxBounces)
		{
			ParticleController particlesCon = GetNode<ParticleController>("/root/ParticleController");
			particlesCon.EmitParticles(this, Scene.BulletCrack);
			Destroy();
		}
	}

	protected override void OnAnythingHit()
	{
		//Do nothing, so the bullet does not get destroyed
	}

	/// <summary>
	/// Collides with a damageable node and deals damage to it
	/// </summary>
	/// <param name="node">The node that the bullet collided with</param>
	public override void OnDamageableHit(Node node)
	{
		if (node is PlayerMovement)//ignore if the player hit himself
		{
			Destroy();
			return;
		}

		node.GetNode<HealthComponent>("HealthComponent").TakeDamage(Damage);
		Destroy();
	}

	/// <summary>
	/// Destroys the bullet when it hits anything other than a damageable node
	/// </summary>
	protected override void OnOtherHit()
	{
		Destroy();
	}

}
