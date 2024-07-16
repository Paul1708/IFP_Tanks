using Godot;

namespace Code.Scripts.Enemies;


/// <summary>
/// Utility which provides navigation functionality to a 2D character.
/// </summary>
public partial class NavigationController : NavigationAgent2D
{
	[Export]
	public float MovementSpeed { get; set; }
	[Export]
	public float RotationSpeed { get; set; } = 0.01f;
	public CharacterBody2D CharacterBody;
	public bool IsMooving { get; set; } = true;
	public override void _Ready()
	{
		CharacterBody = GetParent<CharacterBody2D>();
	}

	/// <summary>
	/// Process function that moves the character towards the target position.
	/// </summary>
	/// <param name="delta">The delta time</param>
	public override void _Process(double delta)
	{
		if (IsMooving)
		{
			var targetVector = GetCurrentNavigationPath().Length > 4 ?
				GetCurrentNavigationPath()[4] : GetNextPathPosition();
			MoveTowardsVector(targetVector);
			return;
		}
		CharacterBody.Velocity = Vector2.Zero;
	}

	/// <summary>
	/// Moves the character towards a target position, lerping the rotation to create a smooth movement.
	/// </summary>
	/// <param name="target">The target position</param>
	public void MoveTowardsVector(Vector2 target)
	{
		// Rotate towards the target position
		var angle = (target - CharacterBody.GlobalPosition).Angle();
		var lerpedAngle = Mathf.LerpAngle(CharacterBody.GlobalRotation, angle, RotationSpeed);
		CharacterBody.GlobalRotation = lerpedAngle;

		// Move forward
		CharacterBody.Velocity = Vector2.Right.Rotated(lerpedAngle) * MovementSpeed;
		CharacterBody.MoveAndSlide();
	}


	/// <summary>
	/// Sets the target position for the character to move towards.
	/// </summary>
	/// <param name="target">The target position</param>
	public void NavigateTowards(Vector2 target)
	{
		TargetPosition = target;
	}
}
