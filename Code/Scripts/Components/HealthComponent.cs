using System.ComponentModel.DataAnnotations;
using Godot;

namespace Components;

public partial class HealthComponent : Node2D
{

    [Export] public float maxHP { get; private set; }
    public float currentHP { get; private set; }

    [Signal]
    public delegate void OnDeathEventHandler();

    public HealthComponent(float maxHealth)
    {
        if (maxHealth <= 0)
        {
            throw new ValidationException("Max health must be greater than 0");
        }

        maxHP = maxHealth;
        currentHP = maxHealth;
    }

    // Parameterless Constructor for Godot
    public HealthComponent() { }

    public void Heal(float value)
    {
        if (value < 0) return;

        //clamp the value to the max health
        currentHP = Mathf.Min(currentHP + value, maxHP);
    }


    public void TakeDamage(float value)
    {
        if (value < 0) return;

        currentHP -= value;

        CheckIfDead();
    }

    public void IncreaseMaxHealth(float value)
    {
        if (value < 0) return;

        maxHP += value;
    }

    public void SetCurrentHP(float value)
    {
        if (value < 0) return;

        //clamp the value to the max health
        currentHP = Mathf.Min(value, maxHP);

        CheckIfDead();
    }



    private void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            GD.Print("Dead");
            EmitSignal(SignalName.OnDeath);
        }
    }

}