using Components;
using Godot;

public partial class EnemyHealthBar : Control
{
    HealthComponent hc;
    public override void _Ready()
    {
        hc = GetParent<HealthComponent>();
        hc.OnHealthChanged += SetHealth;
        hc.OnMaxHealthChanged += SetMaxHealth;

        SetHealth(hc.currentHP);
    }


    public void SetHealth(int health)
    {
        GetNode<Label>("Label").Text = $"{health}/{hc.maxHP}";
    }

    public void SetMaxHealth(int maxHealth)
    {
        GetNode<Label>("Label").Text = $"{hc.currentHP}/{maxHealth}";
    }
}