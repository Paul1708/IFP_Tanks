
using Code.Scripts.Audio;
using Code.Scripts.Components;
using Code.Scripts.Managers;
using Code.Test.Components;
using Godot;


namespace Code.Scripts.Weapons
{
    /// <summary>
    /// Base class for all bullets in the game.
    /// Provides basic functionality for all bullets.
    /// </summary>
    public abstract partial class Bullet : RigidBody2D
    {
        public float Damage { get; set; }
        public Node2D Shooter { get; set; }

        public float Speed { get; set; } //speed of the bullet

        protected Node2D Player;
        protected HealthComponent PlayerHealthComponent;
        protected ParticleController Particles;

        protected Vector2 NormalCollisionVector;
        protected MusicController MusicController;


        public override void _Ready()
        {
            //Destroy bullet, when Destroy Timer runs out
            Timer timer = GetNode<Timer>("DestroyTimer");
            timer.Timeout += () => Destroy();

            // OnCollision Signal
            BodyEntered += OnCollision;

            // other references
            Player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
            PlayerHealthComponent = PlayerManager.Instance.PlayerHealthComponent;
            Particles = GetNode<ParticleController>("/root/ParticleController");
            MusicController = GetNode<MusicController>("/root/MusicController");

            Setup();
        }

        /// <summary>
        /// Method called for the init bullet loading. Used in subclasses to instantiate bullet type specific parameters.
        /// <!---->
        protected virtual void Setup() { }


        /// <summary>
        /// Called when the bullet collides with another node.
        /// Calls the appropriate method for the collision type.
        /// </summary>
        /// <param name="node">The node that the bullet collided with</param>
        public void OnCollision(Node node)
        {
            CameraShaker.Instance.Shake(0.25f, 0.05f);
            OnAnythingHit();
            if (node.IsInGroup("Damageable"))
            {
                OnDamageableHit(node);
            }

            else if (node.IsInGroup("Wall") || node is TileMap)
            {
                OnWallHit();
            }
            else if (node is Bullet otherBullet)
            {
                otherBullet.Destroy();
            }
            else
            {
                OnOtherHit();
            }
        }

        /// <summary>
        /// Called when the bullet hits anything.
        /// </summary>
        protected virtual void OnAnythingHit()
        {
            Destroy();
        }

        /// <summary>
        /// Called when the bullet hits a damageable node.
        /// </summary>
        /// <param name="node">The node that the bullet collided with</param>
        public virtual void OnDamageableHit(Node node)
        {
            if (node == Player)
            {
                CameraShaker.Instance.Shake(5, 0.15f);
                PlayerHealthComponent.TakeDamage(Damage);
                return;
            }
            node.GetNode<HealthComponent>("HealthComponent").TakeDamage(Damage);
        }

        /// <summary>
        /// Called when the bullet hits a wall.
        /// </summary>
        protected virtual void OnWallHit() { }

        /// <summary>
        /// Called when the bullet hits anything other than a damageable node or a wall.
        /// </summary>
        protected virtual void OnOtherHit() { }

        /// <summary>
        /// Describe the move behavior of all bullet types. This method is called on every physic process tick.
        /// </summary>
        protected virtual Node Move()
        {
            return null;

        }

        /// <summary>
        /// Destroys the bullet.
        /// </summary>
        public virtual void Destroy()
        {
            QueueFree();
        }

        /// <summary>
        /// Call move method on every physics process tick and check for collisions.
        /// </summary>
        /// <param name="delta">The time since the last physics process tick</param>
        public override void _PhysicsProcess(double delta)
        {
            Node collided = Move();
            if (collided != null)
                OnCollision(collided);
        }
    }

}


