using Components;
using Godot;
using Managers.Save;

namespace Managers;

public partial class PlayerManager : Node2D
{
    public HealthComponent PlayerHealthComponent { get; private set; }
    public override void _Ready()
    {
        PlayerHealthComponent = GetNode<HealthComponent>("PlayerHealthComponent");
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

