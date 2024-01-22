
using Godot;
using Movement;
using Timer = Godot.Timer;
namespace Weapons
{
    public abstract partial class Bullet : RigidBody2D
    {
        [Export] public float BulletDamage { get; set; }
        [Export] public float BulletSpeed { get; set; } //speed of the bullet

        protected Node2D player;
        protected ParticleController particles;

        protected Vector2 NormalCollisionVector;

        public override void _Ready()
        {
            //Destroy bullet, when Destroy Timer runs out
            Timer timer = GetNode<Timer>("DestroyTimer");
            timer.Timeout += () => Destroy();

            // OnCollision Signal
            BodyEntered += OnCollision;

            // other references
            player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
            particles = GetNode<ParticleController>("/root/ParticleController");

            Setup();
        }

        protected virtual void Setup() { }


        // Collision Methods
        private void OnCollision(Node node)
        {
            OnAnythingHit();
            if (node.IsInGroup("Player"))
            {
                OnPlayerHit();
            }
            else if (node.IsInGroup("Enemy"))
            {
                OnEnemyHit();
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

        protected virtual void OnPlayerHit() { }

        protected virtual void OnWallHit() { }

        protected virtual void OnEnemyHit() { }

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


