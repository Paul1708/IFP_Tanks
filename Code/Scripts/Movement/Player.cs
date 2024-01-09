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
    private float _rotationDirection;


    public override void _Ready()
    {
        Gun = GetNode<GunController>("Gun");
    }

    public override void _PhysicsProcess(double delta)
    {

        //Movement
        GetInput();
        Rotation += _rotationDirection * RotationSpeed * (float)delta;
        MoveAndSlide();

        //Gun controlling
        Gun.RotateTowards(GetGlobalMousePosition());
        if (Input.IsActionJustPressed("left_mouse"))
        {
            Gun.Shoot();
        }
    }

    public void GetInput()
    {
        _rotationDirection = Input.GetAxis("left", "right");
        Velocity = calculateVelocity(Transform.X * Input.GetAxis("down", "up"));
    }

    public Vector2 calculateVelocity(Vector2 move_input)
    {
        Vector2 velocity = move_input * speed; //set the velocity to the input times the speed.
        return velocity;
    }
}
