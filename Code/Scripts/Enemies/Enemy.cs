using Components;
using Godot;

public partial class Enemy : CharacterBody2D
{

	private HealthComponent _healthComponent { get; set; }
	[Export] public PackedScene DropItemScene { get; set; }


	public override void _Ready()
	{
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthComponent.OnDeath += QueueFree;
	}
}
