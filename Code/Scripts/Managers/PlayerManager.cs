using Components;
using Godot;

public partial class PlayerManager : Node2D
{
    public HealthComponent PlayerHealthComponent { get; private set; }
    public override void _Ready()
    {
        PlayerHealthComponent = GetNode<HealthComponent>("PlayerHealthComponent");
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

