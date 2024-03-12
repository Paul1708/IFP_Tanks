using System;
using Components;
using Godot;
using Weapons;
/* This folder and namespace is temporary. I wanted to call it Player, 
but that would conflict with the Player class name. */
namespace Movement;
public partial class Player : CharacterBody2D
{
	[Export] public float speed { get; set; }
	[Export] public float RotationSpeed { get; set; } = 1.5f;
	public GunController Gun { get; set; }
	public AnimationHandler AnimationHandler { get; set; }
	private float _rotationDirection;
	protected TrailComponent trailComponent;
	
	public override void _Ready()
	{
		Gun = GetNode<GunController>("Gun");
		AnimationHandler = GetNode<AnimationHandler>("AnimationPlayer");
		trailComponent = GetNode<TrailComponent>("Trail");
	}

	public override void _PhysicsProcess(double delta)
	{
		//Movement
		GetInput();
		Rotation += _rotationDirection * RotationSpeed * (float)delta;
		MoveAndSlide();

		//Emit trail
		if (IsMoving())
		{
			trailComponent.EmitTrail();
		}
	   
		//Gun controlling
		Gun.RotateTowards(GetGlobalMousePosition());
		if (Input.IsActionPressed("left_mouse"))
		{
			Gun.Shoot();
		}
	}

	public void GetInput()
	{
		Vector2 input = Input.GetVector("left", "right", "down", "up");

		AnimationHandler.PlayAnimationOfInput(input);

		_rotationDirection = input.X;
		Velocity = CalculateVelocity(Transform.X * input.Y);
	}

	public Vector2 CalculateVelocity(Vector2 move_input)
	{
		Vector2 velocity = move_input * speed; //set the velocity to the input times the speed.
		return velocity;
	}

	public bool IsMoving()
	{
		return Velocity != Vector2.Zero;
	}
}
