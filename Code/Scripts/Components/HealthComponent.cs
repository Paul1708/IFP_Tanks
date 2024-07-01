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

    /// <summary>
    /// Heals the entity by the input value
    /// </summary>
    /// <param name="value"></param>
    public void Heal(float value)
    {
        if (value < 0) return;
        SetCurrentHp(CurrentHp + value);
    }

    /// <summary>
    /// Deals damage to the entity by the input value
    /// </summary>
    /// <param name="value"></param>
    public void TakeDamage(float value)
    {
        if (value < 0) return;
        EmitSignal(SignalName.OnTakeDamage, value);
        SetCurrentHp(CurrentHp - value);
    }

    /// <summary>
    /// Heals the entity to the max health
    /// </summary>
    public void HealToMax()
    {
        SetCurrentHp(MaxHp);
    }

    /// <summary>
    /// Heals the entity by the input percentage
    /// </summary>
    /// <param name="percentage"></param>
    public void HealPercentage(float percentage)
    {
        Heal(MaxHp * percentage);
    }

    /// <summary>
    /// Increases the max health of the entity by the input value
    /// </summary>
    /// <param name="value"></param>
    public void IncreaseMaxHealth(float value)
    {
        if (value < 0) return;

        MaxHp += value;
        SetCurrentHp(CurrentHp + value);
        EmitSignal(SignalName.OnMaxHealthChanged, MaxHp);
    }

    /// <summary>
    /// Sets the current health of the entity to the input value
    /// </summary>
    /// <param name="value"></param>
    public void SetCurrentHp(float value)
    {
        //clamp the value to the max health
        CurrentHp = Mathf.Min(value, MaxHp);

        EmitSignal(SignalName.OnHealthChanged, CurrentHp);
        CheckIfDead();
    }

    /// <summary>
    /// Sets the max health of the entity to the input value
    /// </summary>
    /// <param name="value"></param>
    public void SetMaxHp(float value)
    {
        if (value <= 0) return;

        //clamp the value to the max health
        MaxHp = value;
        CurrentHp = Mathf.Min(CurrentHp, MaxHp);
        EmitSignal(SignalName.OnMaxHealthChanged, MaxHp);
    }

    /// <summary>
    /// Checks if the entity is dead (<= 0 health) and emits the OnDeath signal
    /// </summary>
    private void CheckIfDead()
    {
        if (CurrentHp <= 0)
        {
            EmitSignal(SignalName.OnDeath);
        }
    }

}