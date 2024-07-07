using Code.Scripts.Components;
using Godot;
using Code.Scripts.Managers;
using Code.Scripts.Weapons;


namespace Code.Scripts.Movement;

/// <summary>
/// The player movement script. Handles the movement of the player and the gun.
/// </summary>
public partial class PlayerMovement : CharacterBody2D
{
	public GunController Gun { get; set; }
	public AnimationHandler AnimationHandler { get; set; }
	private float _rotationDirection;
	private GpuParticles2D _deathParticles;
	protected TrailComponent TrailComponent;

	public override void _Ready()
	{
		Gun = GetNode<GunController>("Gun");
		AnimationHandler = GetNode<AnimationHandler>("AnimationPlayer");
		TrailComponent = GetNode<TrailComponent>("TrailComponent");
		_deathParticles = GetNode<GpuParticles2D>("DeathParticles");
		PlayerManager.Instance.PlayerHealthComponent.OnDeath += OnDeath;
	}

	/// <summary>
	/// Get the input from the player and set the velocity accordingly and rotate the gun.
	/// </summary>
	/// <param name="delta"> The time since the last frame</param>
	public override void _PhysicsProcess(double delta)
	{
		//Movement
		GetInput();
		Rotation += _rotationDirection * PlayerManager.Instance.PlayerStats.CurrentRotationSpeed * (float)delta;
		MoveAndSlide();

		//Emit trail
		if (IsMoving())
		{
			TrailComponent.EmitTrail();
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

	/// <summary>
	/// Get the input from the player and set the velocity accordingly.
	/// </summary>
	public void GetInput()
	{
		Vector2 input = Input.GetVector("left", "right", "down", "up");

		AnimationHandler.PlayAnimationOfInput(input);

		_rotationDirection = input.X;
		Velocity = CalculateVelocity(Transform.X * input.Y);
	}

	/// <summary>
	/// Calculate the velocity of the player based on the input.
	/// </summary>
	/// <param name="moveInput">The input from the player</param>
	/// <returns>The velocity of the player</returns>
	public Vector2 CalculateVelocity(Vector2 moveInput)
	{
		Vector2 velocity = moveInput * PlayerManager.Instance.PlayerStats.CurrentMovementSpeed; //set the velocity to the input times the speed.
		return velocity;
	}

	/// <summary>
	/// Check if the player is moving.
	/// </summary>
	/// <returns>True if the player is moving, false otherwise</returns>
	public bool IsMoving()
	{
		return Velocity != Vector2.Zero;
	}

	/// <summary>
	/// Called when the player dies. Emits the death particles and queues the player for deletion.
	/// </summary>
	private async void OnDeath()
	{
		_deathParticles.Emitting = true;
		await ToSignal(_deathParticles, "finished");
		QueueFree();
	}
}
