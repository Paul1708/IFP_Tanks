using Components;
using Godot;
using System;

public partial class DummyTank : CharacterBody2D
{
	TrailComponent trailComponent;

	public override void _Ready()
	{
		trailComponent = GetNode<TrailComponent>("TrailComponent");
	}

	public override void _PhysicsProcess(double delta)
	{
		//Emit trail
		if (IsMoving())
		{
			trailComponent.EmitTrail();
		}
	}

	private bool IsMoving()
	{
		return Velocity != Vector2.Zero;
	}
}
