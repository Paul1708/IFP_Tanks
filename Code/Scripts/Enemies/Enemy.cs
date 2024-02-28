using Components;
using Godot;

public partial class Enemy : CharacterBody2D
{
	private HealthComponent _healthComponent { get; set; }
	[Export] public PackedScene DropItemScene { get; set; }

	public override void _Ready()
	{
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthComponent.OnDeath += OnDeath;
	}

	public override void _ExitTree()
	{
		_healthComponent.OnDeath -= QueueFree;
	}

	public void OnDeath()
	{
		// Disable collision, so the enemy can't be hit anymore
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);

		// Tween enemies scale to zero and queue free
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", Vector2.Zero, 0.5f)
			.SetTrans(Tween.TransitionType.Back);

		tween.Finished += QueueFree;
	}
}
