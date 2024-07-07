using Code.Scripts.Managers;
using Godot;


namespace Code.Scripts.Weapons
{
	/// <summary>
	/// Controls the basic (50cal) bullet
	/// </summary>
	public partial class BasicBullet : Bullet
	{
		/// <summary>
		/// Moves the bullet forward and returns the collided node, if any
		/// </summary>
		/// <returns>The node that the bullet collided with</returns>
		protected override Node Move()
		{
			KinematicCollision2D collided = MoveAndCollide(LinearVelocity);
			if (collided != null)
			{
				var collider = collided.GetCollider();
				if (collider is Node)
				{
					return collider as Node;
				}
			}

			return null;
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
		/// Fires when the bullet collides with other nodes
		/// </summary>
		protected override void OnOtherHit()
		{
			Destroy();
		}
	}

}


