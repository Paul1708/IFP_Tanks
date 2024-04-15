using Godot;

namespace Components;
public partial class DamageNumber : RigidBody2D
{
    public string Text { get; set; }
    public Label label { get; set; }
    private float lifeTime = 0.65f;        // The lifetime of the damage number
    private float rotationSpeed;        // How much the damage number rotates
    private float horizontalSpeed;      // How fast the damage number moves horizontally
    private float verticalSpeed = -300; // The initial vertical speed of the damage number


    public override void _Ready()
    {
        // Create a timer that will destroy the damage number after its lifetime
        var timer = GetTree().CreateTimer(lifeTime);
        timer.Timeout += QueueFree;

        // Set the text of the label to the damage value
        label = GetNode<Label>("Label");
        label.Text = Text;

        // Set the rotation speed and the horizontal speed to random values
        var random = new RandomNumberGenerator();
        rotationSpeed = random.RandfRange(-10, 10);
        horizontalSpeed = rotationSpeed * 10;
        LinearVelocity = new Vector2(horizontalSpeed, verticalSpeed);

        // Create a tween that will scale the damage number from 1 to 0.2 in its lifetime
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(label, "scale", Vector2.One * 0.2f, 1f)
            .SetTrans(Tween.TransitionType.Back);
    }


    public override void _PhysicsProcess(double delta)
    {
        // Rotate the damage number
        Rotation = Rotation + rotationSpeed * (float)delta;
    }
}