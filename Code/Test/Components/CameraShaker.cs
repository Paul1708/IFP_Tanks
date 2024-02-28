using Godot;

public partial class CameraShaker : Camera2D
{
    public static CameraShaker Instance { get; private set; }
    float shakeAmmount = 0;
    Vector2 defaultOffset;
    RandomNumberGenerator random = new RandomNumberGenerator();
    Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("ShakeTimer");
        timer.Timeout += StopShake;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }

        SetProcess(false);
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        timer.Timeout -= StopShake;
    }
    public override void _Process(double delta)
    {
        Offset = new Vector2(random.RandfRange(-1, 1) * shakeAmmount, random.RandfRange(-1, 1) * shakeAmmount);
    }

    public void Shake(float ammount, float duration)
    {
        timer.Start(duration);
        shakeAmmount = ammount;
        SetProcess(true);
    }

    private void StopShake()
    {
        // Tween to smoothly stop the shake
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "offset", Vector2.Zero, 0.1)
         .SetTrans(Tween.TransitionType.Expo)
         .SetEase(Tween.EaseType.Out); ;
        SetProcess(false);
    }



}