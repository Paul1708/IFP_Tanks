
using Code.Scripts.Audio;
using Code.Scripts.Components;
using Code.Scripts.Managers;
using Code.Test.Components;
using Godot;

namespace Code.Scripts.Weapons
{
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

        /**
         * Method called for the init bullet loading. Used in subclasses to instantiate bullet type specific parameters.
         */
        protected virtual void Setup() { }


        // Collision Methods
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

        protected virtual void OnAnythingHit()
        {
            Destroy();
        }

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

        protected virtual void OnWallHit() { }

        protected virtual void OnOtherHit() { }

        /**
         * Describe the move behavior of all bullet types. This method is called on every physic process tick.
         */
        protected virtual Node Move()
        {
            return null;

        }


        public virtual void Destroy()
        {
            QueueFree();
        }

        public override void _PhysicsProcess(double delta)
        {
            Node collided = Move();
            if (collided != null)
                OnCollision(collided);
        }
    }

}


