using System;
using Godot;

namespace Components;

public partial class AnimationHandler : AnimationPlayer
{
    public void PlayAnimationOfInput(Vector2 input)
    {

        if (input == Vector2.Zero)
        {
            Stop();
            return;
        }

        if (Math.Abs(input.Y) >= Math.Abs(input.X))
        {
            Play(input.Y > 0 ? "Forward" : "Backward");
        }
        else
        {
            Play(input.X > 0 ? "Right" : "Left");
        }

    }

    public void PlayReset()
    {
        Play("RESET");
    }
}

