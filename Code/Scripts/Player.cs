using Godot;

public partial class Player : CharacterBody2D
{
    [Export] public float speed { get; set; }
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

        Velocity = calculateVelocity(Input.GetVector("left", "right", "up", "down"));

        MoveAndSlide(); //move and slide the player
    }

    public Vector2 calculateVelocity(Vector2 move_input)
    {
        Vector2 velocity = move_input * speed; //set the velocity to the input times the speed.
        return velocity;
    }
}
