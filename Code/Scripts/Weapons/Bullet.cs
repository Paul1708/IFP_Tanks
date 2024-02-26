
using Components;
using Godot;
using Timer = Godot.Timer;
namespace Weapons
{
    public abstract partial class Bullet : RigidBody2D
    {
        public float Damage { get; set; }
        [Export] public float Speed { get; set; } //speed of the bullet

        public Node2D Player;
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

        public virtual void OnDamageableHit(Node node)
        {
            node.GetNode<HealthComponent>("HealthComponent").TakeDamage(Damage);
        }

        protected virtual void OnWallHit() { }

        protected virtual void OnOtherHit() { }

        /**
         * Describe the move behavior of all bullet types. This method is called on every physic process tick.
         */
        protected virtual void Move() { }
        

        public virtual void Destroy()
        {
            QueueFree();
        }


        public override void _PhysicsProcess(double delta)
        {
            Move();
        }
    }

}


