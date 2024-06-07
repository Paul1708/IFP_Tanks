
using Code.Scripts.Components;
using Code.Scripts.Managers;
using Code.Scripts.Movement;
using Godot;

namespace Code.Scripts.Weapons;

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

	public override void Destroy()
	{
		Particles.EmitParticles(this, Scene.BulletCrack);
		QueueFree();
	}

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
	protected override void OnOtherHit()
	{
		Destroy();
	}

}
