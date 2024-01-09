
using Godot;
using System;

public partial class NavigationController : NavigationAgent2D
{
	[Export]
	public float MovementSpeed { get; set; }
	private CharacterBody2D characterBody;
	public override void _Ready()
	{
		characterBody = GetParent<CharacterBody2D>();
	}

	public override void _Process(double delta)
	{
		// Move towards the target position
		Vector2 direction = (GetNextPathPosition() - characterBody.GlobalPosition).Normalized();
		characterBody.Velocity = direction * MovementSpeed;
		characterBody.MoveAndSlide();

		characterBody.LookAt(GetNextPathPosition());
	}


	// Sets the target position to navigate towards
	public void NavigateTowards(Vector2 target)
	{
		TargetPosition = target;
	}
}
