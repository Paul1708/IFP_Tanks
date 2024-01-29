using System.ComponentModel.DataAnnotations;
using Godot;

namespace Components;

public partial class HealthComponent : Node2D
{

    [Export] public float maxHP { get; set; }
    public float currentHP { get; set; }

    [Signal]
    public delegate void OnDeathEventHandler();

    public override void _Ready()
    {
        if (maxHP <= 0)
        {
            throw new ValidationException("Max health must be greater than 0");
        }

        currentHP = maxHP;
    }

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
        GD.Print("Health: " + currentHP + "/" + maxHP);

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
            EmitSignal(SignalName.OnDeath);
        }
    }

}