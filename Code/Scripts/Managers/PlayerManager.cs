using Code.Scripts.Movement;
using Code.Scripts.Components;
using Godot;
using Code.Scripts.Managers.Save;

namespace Code.Scripts.Managers;

/// <summary>
/// Manages the player's stats and health
/// </summary>
public partial class PlayerManager : Node2D
{
    public static PlayerManager Instance { get; private set; }
    public HealthComponent PlayerHealthComponent { get; private set; }
    public PlayerStats PlayerStats { get; set; }

    [Signal]
    public delegate void OnPlayerStatsChangedEventHandler(PlayerStats stats);
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            QueueFree(); // Ensures there is only one instance of PlayerManager
        }

        PlayerHealthComponent = GetNode<HealthComponent>("PlayerHealthComponent");
        SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;
    }

    public override void _ExitTree()
    {
        SaveManager.Instance.OnSaveDataLoaded -= OnSaveDataLoaded;
    }

    /// <summary>
    /// Adds the input amount to the player's damage modifier
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public void AddDamageModifier(float amount)
    {
        PlayerStats.CurrentDamageModifier += amount;
        EmitSignal(SignalName.OnPlayerStatsChanged, PlayerStats);
    }

    /// <summary>
    /// Adds the input amount to the player's movement speed
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public void AddMovementSpeed(float amount)
    {
        PlayerStats.CurrentMovementSpeed += amount;
        EmitSignal(SignalName.OnPlayerStatsChanged, PlayerStats);
    }

    /// <summary>
    /// Adds the input amount to the player's rotation speed
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public void AddRotationSpeed(float amount)
    {
        PlayerStats.CurrentRotationSpeed += amount;
        EmitSignal(SignalName.OnPlayerStatsChanged, PlayerStats);
    }

    /// <summary>
    /// Adds the input amount to the player's max health
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public void AddMaxHealth(float amount)
    {
        PlayerStats.CurrentMaxHealth += amount;
        PlayerHealthComponent.IncreaseMaxHealth(amount);
    }

    /// <summary>
    /// Resets the player's max health to the base max health
    /// </summary>
    public void ResetMaxHealth()
    {
        PlayerHealthComponent.SetMaxHp(PlayerStats.BaseMaxHealth);
    }

    /// <summary>
    /// Loads the player's stats from the save data
    /// </summary>
    /// <param name="saveData">The save data that is loaded</param>
    public void OnSaveDataLoaded(SaveData saveData)
    {
        PlayerStats = saveData.PlayerStats;
        PlayerHealthComponent.SetCurrentHp(saveData.PlayerCurrentHp);
        PlayerHealthComponent.SetMaxHp(PlayerStats.CurrentMaxHealth);
    }
}

