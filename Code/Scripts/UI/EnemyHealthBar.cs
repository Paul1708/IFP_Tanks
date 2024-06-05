using System;
using Code.Scripts.Components;
using Godot;

namespace Code.Scripts.UI;

public partial class EnemyHealthBar : Control
{
    private HealthComponent _hc;

    public override void _Ready()
    {
        _hc = GetParent<HealthComponent>();
        _hc.OnHealthChanged += SetHealth;
        _hc.OnMaxHealthChanged += SetMaxHealth;

        SetHealth(_hc.CurrentHp);
    }


    public void SetHealth(float health)
    {
        GetNode<Label>("Label").Text = $"{Math.Round(health, 1)}/{_hc.MaxHp}";
    }

    public void SetMaxHealth(float maxHealth)
    {
        GetNode<Label>("Label").Text = $"{Math.Round(_hc.CurrentHp, 1)}/{maxHealth}";
    }
}