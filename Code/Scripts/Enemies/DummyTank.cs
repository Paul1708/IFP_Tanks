using Components;
using Godot;
using System;

public partial class DummyTank : CharacterBody2D
{
	TrailComponent trailComponent;
	public AnimationHandler AnimationHandler { get; set; }


	public override void _Ready()
	{
		trailComponent = GetNode<TrailComponent>("TrailComponent");
		AnimationHandler = GetNode<AnimationHandler>("AnimationPlayer");

	}

	public override void _PhysicsProcess(double delta)
	{
		//Emit trail
		if (IsMoving())
		{
			trailComponent.EmitTrail();
		}
		Vector2 movement = this.Velocity;
		AnimationHandler.PlayAnimationOfInput(movement);
	}

	private bool IsMoving()
	{
		return Velocity != Vector2.Zero;
	}
}
