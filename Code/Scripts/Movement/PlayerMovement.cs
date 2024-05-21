using Components;
using Godot;
using Managers;
using Weapons;
/* This folder and namespace is temporary. I wanted to call it Player, 
but that would conflict with the Player class name. */
namespace Player;
public partial class PlayerMovement : CharacterBody2D
{
	public GunController Gun { get; set; }
	public AnimationHandler AnimationHandler { get; set; }
	private float _rotationDirection;
	private GpuParticles2D _deathParticles;
	protected TrailComponent trailComponent;

	public override void _Ready()
	{
		Gun = GetNode<GunController>("Gun");
		AnimationHandler = GetNode<AnimationHandler>("AnimationPlayer");
		trailComponent = GetNode<TrailComponent>("TrailComponent");
		_deathParticles = GetNode<GpuParticles2D>("DeathParticles");
		PlayerManager.Instance.PlayerHealthComponent.OnDeath += OnDeath;
	}

	public override void _PhysicsProcess(double delta)
	{
		//Movement
		GetInput();
		Rotation += _rotationDirection * PlayerManager.Instance.PlayerStats.CurrentRotationSpeed * (float)delta;
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

	public override void _ExitTree()
	{
		PlayerManager.Instance.PlayerHealthComponent.OnDeath -= OnDeath;
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
		Vector2 velocity = move_input * PlayerManager.Instance.PlayerStats.CurrentMovementSpeed; //set the velocity to the input times the speed.
		return velocity;
	}

	public bool IsMoving()
	{
		return Velocity != Vector2.Zero;
	}

	private async void OnDeath()
	{
		_deathParticles.Emitting = true;
		await ToSignal(_deathParticles, "finished");
		QueueFree();
	}
}
