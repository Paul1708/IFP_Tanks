using System.ComponentModel.DataAnnotations;
using Godot;

namespace Code.Scripts.Components;

public partial class HealthComponent : Node2D
{

    [Export] public float MaxHp { get; set; } = 100;
    public float CurrentHp { get; set; }
    [Signal]
    public delegate void OnDeathEventHandler();
    [Signal]
    public delegate void OnHealthChangedEventHandler(float currentHp);
    [Signal]
    public delegate void OnMaxHealthChangedEventHandler(float maxHp);
    [Signal]
    public delegate void OnTakeDamageEventHandler(float damage);
    public override void _Ready()
    {
        if (MaxHp <= 0)
        {
            throw new ValidationException("Max health must be greater than 0");
        }

        SetCurrentHp(MaxHp);
    }

    public void Heal(float value)
    {
        if (value < 0) return;
        SetCurrentHp(CurrentHp + value);
    }

    public void TakeDamage(float value)
    {
        if (value < 0) return;
        EmitSignal(SignalName.OnTakeDamage, value);
        SetCurrentHp(CurrentHp - value);
    }

    public void HealToMax()
    {
        SetCurrentHp(MaxHp);
    }

    public void HealPercentage(float percentage)
    {
        Heal(MaxHp * percentage);
    }

    public void IncreaseMaxHealth(float value)
    {
        if (value < 0) return;

        MaxHp += value;
        SetCurrentHp(CurrentHp + value);
        EmitSignal(SignalName.OnMaxHealthChanged, MaxHp);
    }

    public void SetCurrentHp(float value)
    {
        //clamp the value to the max health
        CurrentHp = Mathf.Min(value, MaxHp);

        EmitSignal(SignalName.OnHealthChanged, CurrentHp);
        CheckIfDead();
    }

    public void SetMaxHp(float value)
    {
        if (value <= 0) return;

        //clamp the value to the max health
        MaxHp = value;
        CurrentHp = Mathf.Min(CurrentHp, MaxHp);
        EmitSignal(SignalName.OnMaxHealthChanged, MaxHp);
    }

    private void CheckIfDead()
    {
        if (CurrentHp <= 0)
        {
            EmitSignal(SignalName.OnDeath);
        }
    }

}