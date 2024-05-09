using Godot;

public partial class NavigationController : NavigationAgent2D
{
	[Export]
	public float MovementSpeed { get; set; }
	[Export]
	public float RotationSpeed { get; set; } = 0.01f;
	public CharacterBody2D characterBody;
	public bool IsMooving { get; set; } = true;
	public override void _Ready()
	{
		characterBody = GetParent<CharacterBody2D>();
	}

	public override void _Process(double delta)
	{
		if (IsMooving)
		{
			MoveTowardsVector(GetNextPathPosition());
			return;
		}
		characterBody.Velocity = Vector2.Zero;
	}

	public void MoveTowardsVector(Vector2 target)
	{
		// Rotate towards the target position
		var angle = (target - characterBody.GlobalPosition).Angle();
		var lerpedAngle = Mathf.LerpAngle(characterBody.GlobalRotation, angle, RotationSpeed);
		characterBody.GlobalRotation = lerpedAngle;

		// Move forward
		characterBody.Velocity = Vector2.Right.Rotated(lerpedAngle) * MovementSpeed;
		characterBody.MoveAndSlide();
	}


	// Sets the target position to navigate towards
	public void NavigateTowards(Vector2 target)
	{
		TargetPosition = target;
	}
}
