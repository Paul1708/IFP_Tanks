using Godot;
using System;

public partial class player : CharacterBody2D
{
    [Export] public float speed = 300f;
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

        Vector2 move_input = Input.GetVector("left", "right", "up", "down"); //create a vector2 with the input

        Velocity = move_input * speed; //set the velocity to the input times the speed

        MoveAndSlide(); //move and slide the player
    }
}
