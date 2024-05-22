using Components;
using Godot;
using Managers.Save;
using Player;

namespace Managers;

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

    public void AddDamageModifier(float amount)
    {
        PlayerStats.CurrentDamageModifier += amount;
        EmitSignal(SignalName.OnPlayerStatsChanged, PlayerStats);
    }

    public void AddMovementSpeed(float amount)
    {
        PlayerStats.CurrentMovementSpeed += amount;
        EmitSignal(SignalName.OnPlayerStatsChanged, PlayerStats);
    }

    public void AddRotationSpeed(float amount)
    {
        PlayerStats.CurrentRotationSpeed += amount;
        EmitSignal(SignalName.OnPlayerStatsChanged, PlayerStats);
    }

    public void AddMaxHealth(float amount)
    {
        PlayerStats.CurrentMaxHealth += amount;
        PlayerHealthComponent.IncreaseMaxHealth(amount);
    }
    public void ResetMaxHealth()
    {
        PlayerHealthComponent.SetMaxHP(PlayerStats.BaseMaxHealth);
    }

    public void OnSaveDataLoaded(SaveData saveData)
    {
        PlayerStats = saveData.PlayerStats;
        PlayerHealthComponent.SetCurrentHP(saveData.PlayerCurrentHP);
        PlayerHealthComponent.SetMaxHP(PlayerStats.CurrentMaxHealth);
    }
}

