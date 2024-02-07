using Components;
using Godot;
using Managers.Save;

namespace Managers;

public partial class PlayerManager : Node2D
{
    public static PlayerManager Instance { get; private set; }
    public HealthComponent PlayerHealthComponent { get; private set; }
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

    public void OnSaveDataLoaded(SaveData saveData)
    {
        PlayerHealthComponent.SetCurrentHP(saveData.PlayerCurrentHP);
        PlayerHealthComponent.SetMaxHP(saveData.PlayerMaxHP);
    }
    // TODO: Remove this Debug method
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Debug2"))
        {
            PlayerHealthComponent.TakeDamage(10);
        }
    }
}

