
using Components;
using Godot;
using Managers;

namespace Weapons
{
    public abstract partial class Bullet : RigidBody2D
    {
        public int damage { get; set; }
        [Export] public float Speed { get; set; } //speed of the bullet

        protected Node2D player;
        protected HealthComponent playerHealthComponent;
        protected ParticleController particles;

        protected Vector2 NormalCollisionVector;
        protected MusicController musicController;


        public override void _Ready()
        {
            //Destroy bullet, when Destroy Timer runs out
            Timer timer = GetNode<Timer>("DestroyTimer");
            timer.Timeout += () => Destroy();

            // OnCollision Signal
            BodyEntered += OnCollision;

            // other references
            player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
            playerHealthComponent = PlayerManager.Instance.PlayerHealthComponent;
            particles = GetNode<ParticleController>("/root/ParticleController");
            musicController = GetNode<MusicController>("/root/MusicController");

            Setup();
        }

        protected virtual void Setup() { }


        // Collision Methods
        private void OnCollision(Node node)
        {
            CameraShaker.Instance.Shake(0.25f, 0.05f);
            OnAnythingHit();
            if (node.IsInGroup("Damageable"))
            {
                OnDamageableHit(node);
            }
            else if (node.IsInGroup("Wall"))
            {
                OnWallHit();
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

        protected virtual void OnDamageableHit(Node node)
        {
            if (node == player)
            {
                CameraShaker.Instance.Shake(5, 0.15f);
                playerHealthComponent.TakeDamage(damage);
                return;
            }
            node.GetNode<HealthComponent>("HealthComponent").TakeDamage(damage);
        }

        protected virtual void OnWallHit() { }

        protected virtual void OnOtherHit() { }

        protected virtual void Move() { }



        public virtual void Destroy()
        {
            particles.EmitParticles(this, Scene.BulletCrack);
            QueueFree();
        }


        public override void _PhysicsProcess(double delta)
        {
            Move();
        }
    }

}


