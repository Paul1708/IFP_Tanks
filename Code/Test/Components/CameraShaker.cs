using Godot;

namespace Code.Test.Components;

public partial class CameraShaker : Camera2D
{
    public static CameraShaker Instance { get; private set; }
    float _shakeAmount;
    Vector2 _defaultOffset;
    RandomNumberGenerator _random = new ();
    Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("ShakeTimer");
        _timer.Timeout += StopShake;

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
        _timer.Timeout -= StopShake;
    }
    public override void _Process(double delta)
    {
        Offset = new Vector2(_random.RandfRange(-1, 1) * _shakeAmount, _random.RandfRange(-1, 1) * _shakeAmount);
    }

    public void Shake(float ammount, float duration)
    {
        _timer.Start(duration);
        _shakeAmount = ammount;
        SetProcess(true);
    }

    private void StopShake()
    {
        // Tween to smoothly stop the shake
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "offset", Vector2.Zero, 0.1)
         .SetTrans(Tween.TransitionType.Expo)
         .SetEase(Tween.EaseType.Out);
        SetProcess(false);
    }



}