using Godot;
/* This folder and namespace is temporary. I wanted to call it Player, 
but that would conflict with the Player class name. */
namespace Movement;
public partial class Player : CharacterBody2D
{
    [Export] public float speed { get; set; }
    [Export] public float RotationSpeed { get; set; } = 1.5f;
    private float _rotationDirection;
    public Vector2 ScreenSize; // Size of the game window.

    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size; //get the size of the screen
    }

    public override void _PhysicsProcess(double delta)
    {
        //limit the player to the screen
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );

        //Movement
        GetInput();
        Rotation += _rotationDirection * RotationSpeed * (float)delta;
        MoveAndSlide();
    }

    public void GetInput()
    {
        _rotationDirection = Input.GetAxis("left", "right");
        Velocity = calculateVelocity(Transform.X * Input.GetAxis("down", "up"));
    }

    public Vector2 calculateVelocity(Vector2 move_input)
    {
        Vector2 velocity = move_input * speed; //set the velocity to the input times the speed.
        GD.Print(velocity, speed);
        return velocity;
    }
}
