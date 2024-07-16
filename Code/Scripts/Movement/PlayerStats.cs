using Godot;

namespace Code.Scripts.Movement;

/// <summary>
/// Data class which holds the base and current player stats
/// </summary>
[GlobalClass]
public partial class PlayerStats : Resource
{
    [Export] public float BaseMovementSpeed;
    [Export] public float CurrentMovementSpeed;
    [Export] public float BaseRotationSpeed;
    [Export] public float CurrentRotationSpeed;
    [Export] public float BaseDamageModifier;
    [Export] public float CurrentDamageModifier;
    [Export] public float BaseMaxHealth;
    [Export] public float CurrentMaxHealth;
}