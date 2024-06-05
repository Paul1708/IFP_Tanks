using Godot;

namespace Code.Scripts.Environment;
public partial class DamageNumber : RigidBody2D
{
    public string Text { get; set; }
    public Label Label { get; set; }
    private float _lifeTime = 0.65f;        // The lifetime of the damage number
    private float _rotationSpeed;        // How much the damage number rotates
    private float _horizontalSpeed;      // How fast the damage number moves horizontally
    private float _verticalSpeed = -300; // The initial vertical speed of the damage number


    public override void _Ready()
    {
        // Create a timer that will destroy the damage number after its lifetime
        var timer = GetTree().CreateTimer(_lifeTime);
        timer.Timeout += QueueFree;

        // Set the text of the label to the damage value
        Label = GetNode<Label>("Label");
        Label.Text = Text;

        // Set the rotation speed and the horizontal speed to random values
        var random = new RandomNumberGenerator();
        _rotationSpeed = random.RandfRange(-10, 10);
        _horizontalSpeed = _rotationSpeed * 10;
        LinearVelocity = new Vector2(_horizontalSpeed, _verticalSpeed);

        // Create a tween that will scale the damage number from 1 to 0.2 in its lifetime
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Label, "scale", Vector2.One * 0.2f, 1f)
            .SetTrans(Tween.TransitionType.Back);
    }


    public override void _PhysicsProcess(double delta)
    {
        // Rotate the damage number
        Rotation = Rotation + _rotationSpeed * (float)delta;
    }
}