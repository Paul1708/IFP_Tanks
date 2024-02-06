using System.ComponentModel.DataAnnotations;
using Godot;

namespace Components;

public partial class HealthComponent : Node2D
{

    [Export] public int maxHP { get; set; }
    public int currentHP { get; set; }

    [Signal]
    public delegate void OnDeathEventHandler();

    [Signal]
    public delegate void OnHealthChangedEventHandler(int currentHP);
    [Signal]
    public delegate void OnMaxHealthChangedEventHandler(int maxHP);

    public override void _Ready()
    {
        if (maxHP <= 0)
        {
            throw new ValidationException("Max health must be greater than 0");
        }

        SetCurrentHP(maxHP);
    }

    public void Heal(int value)
    {
        if (value < 0) return;
        SetCurrentHP(currentHP + value);
    }


    public void TakeDamage(int value)
    {
        if (value < 0) return;
        SetCurrentHP(currentHP - value);
    }

    public void HealToMax()
    {
        SetCurrentHP(maxHP);
    }

    public void IncreaseMaxHealth(int value)
    {
        if (value < 0) return;

        SetCurrentHP(currentHP + value);
        maxHP += value;
        EmitSignal(SignalName.OnMaxHealthChanged, maxHP);
    }

    public void SetCurrentHP(int value)
    {
        //clamp the value to the max health
        currentHP = Mathf.Min(value, maxHP);

        EmitSignal(SignalName.OnHealthChanged, currentHP);

        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            EmitSignal(SignalName.OnDeath);
        }
    }

}