using System;
using Components;
using Godot;
using Managers;

namespace Enemies;

public partial class KamikazeBehaviour : Behaviour
{
    
    [Export] public float ExplosionDamage;
    [Export] public float ExplosionDamageRadius;

    [Export] public float TargetReachedDistance;

    private bool _exploded;
    
    
    //TODO: Bullets from enemies cannot hit other enemies
    //TODO: When two bullets of same group i.e. shot by enemy collide, destroy the weaker one, if they are the same type delete a random one
    
    public override void Setup()
    {
        Navigation.NavigateTowards(Player.GlobalPosition);
    }

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

    private void _explode()
    {
        HealthComponent hc = Enemy.GetNode<HealthComponent>("HealthComponent");
        hc.TakeDamage(hc.maxHP);
        

        //Damage all close enemies
        foreach (Node node in GetTree().GetNodesInGroup("Damageable"))
        {
            if (node is not Node2D) continue;

            float distance = (((Node2D)node).GlobalPosition - Enemy.GlobalPosition).Length();
            if (node is CharacterBody2D && distance <= ExplosionDamageRadius)
            {
                float finalDamage = ExplosionDamage / distance;
                _takeDamage(node, (int) finalDamage);
            }
        }
        particles.EmitParticles(this, Scene.KamikazeExplosion);
        MusicController.Play(Sound.RocketExplosion);
    }

    private void _takeDamage(Node node, int damage)
    {
        if (node == Player)
            PlayerManager.Instance.PlayerHealthComponent.TakeDamage(damage);

        else
        {
            HealthComponent hc = node.GetNode<HealthComponent>("HealthComponent");
            if(hc != null) hc.TakeDamage(damage);
        }
            
    }
}