using Godot;

namespace Weapons
{
    public partial class Bullet : RigidBody2D
    {
        public override void _Ready()
        {
            Timer timer = GetNode<Timer>("Timer");
            timer.Timeout += () => QueueFree(); //queue free the bullet when the timer runs out.

            BodyEntered += OnHit; //when the bullet hits something, call OnHit
        }

        private void OnHit(Node node)
        {
            QueueFree(); // Destroy the bullet
        }
    }
}

