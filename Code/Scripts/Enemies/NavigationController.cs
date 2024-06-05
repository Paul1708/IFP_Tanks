using Godot;

namespace Code.Scripts.Enemies;

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


	// Sets the target position to navigate towards
	public void NavigateTowards(Vector2 target)
	{
		TargetPosition = target;
	}
}
