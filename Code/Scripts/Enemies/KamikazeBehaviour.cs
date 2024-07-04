using Code.Scripts.Audio;
using Code.Scripts.Components;
using Godot;
using Code.Scripts.Managers;

namespace Code.Scripts.Enemies;


/// <summary>
/// Invis behaivour of the kamikaze (yellow) enemy.
/// </summary>
public partial class KamikazeBehaviour : Behaviour
{

    [Export] public float ExplosionDamage;
    [Export] public float ExplosionDamageRadius;

    [Export] public float TargetReachedDistance;

    private bool _exploded;
    

    public override void Setup()
    {
        Navigation.NavigateTowards(Player.GlobalPosition);
    }

    /// <summary>
    /// Executes the behaivour of the enemy.
    /// The enemy will move towards the player and explode when it touches the player.
    /// </summary>
    public override void ExecuteBehaivour()
    {
        if ((Enemy.GlobalPosition - Player.GlobalPosition).Length() <= TargetReachedDistance)
        {
            if (!_exploded)
            {
                _explode();
                _exploded = true;
            }
            return;
        }

        Navigation.NavigateTowards(Player.GlobalPosition);
    }


    /// <summary>
    /// Explodes the kamikaze enemy, dealing damage to all close enemies and killing itself.
    /// </summary>
    private void _explode()
    {
        HealthComponent hc = Enemy.GetNode<HealthComponent>("HealthComponent");
        hc.TakeDamage(hc.MaxHp);


        //Damage all close enemies
        foreach (Node node in GetTree().GetNodesInGroup("Damageable"))
        {
            if (node is not Node2D) continue;

            float distance = (((Node2D)node).GlobalPosition - Enemy.GlobalPosition).Length();
            if (node is CharacterBody2D && distance <= ExplosionDamageRadius)
            {
                float finalDamage = ExplosionDamage / distance;
                _takeDamage(node, (int)finalDamage);
            }
        }
        Particles.EmitParticles(this, Scene.KamikazeExplosion);
        MusicController.Play(Sound.RocketExplosion);
    }

    /// <summary>
    /// Add damge to the node, which can be the player or another enemy.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="damage"></param>
    private void _takeDamage(Node node, int damage)
    {
        if (node == Player)
            PlayerManager.Instance.PlayerHealthComponent.TakeDamage(damage);

        else
        {
            HealthComponent hc = node.GetNode<HealthComponent>("HealthComponent");
            if (hc != null) hc.TakeDamage(damage);
        }

    }
}