using Components;
using Godot;

public partial class EnemyHealthBar : Control
{
    private HealthComponent _hc;
    
    public override void _Ready()
    {
        _hc = GetParent<HealthComponent>();
        _hc.OnHealthChanged += SetHealth;
        _hc.OnMaxHealthChanged += SetMaxHealth;

        SetHealth(_hc.currentHP);
    }


    public void SetHealth(int health)
    {
        GetNode<Label>("Label").Text = $"{health}/{_hc.maxHP}";
    }

    public void SetMaxHealth(int maxHealth)
    {
        GetNode<Label>("Label").Text = $"{_hc.currentHP}/{maxHealth}";
    }
}