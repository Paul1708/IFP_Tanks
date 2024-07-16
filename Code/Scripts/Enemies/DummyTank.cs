using Code.Scripts.Components;
using Godot;

namespace Code.Scripts.Enemies;

/// <summary>
/// Provides trail and animation handling for the Tank on the main menu
/// </summary>
public partial class DummyTank : CharacterBody2D
{
	private TrailComponent _trailComponent;
	public AnimationHandler AnimationHandler { get; set; }


	public override void _Ready()
	{
		_trailComponent = GetNode<TrailComponent>("TrailComponent");
		AnimationHandler = GetNode<AnimationHandler>("AnimationPlayer");

	}

	public override void _PhysicsProcess(double delta)
	{
		//Emit trail
		if (IsMoving())
		{
			_trailComponent.EmitTrail();
		}
		Vector2 movement = Velocity;
		AnimationHandler.PlayAnimationOfInput(movement);
	}

	private bool IsMoving()
	{
		return Velocity != Vector2.Zero;
	}
}
