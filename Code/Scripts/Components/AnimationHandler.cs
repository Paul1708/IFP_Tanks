using System;
using Godot;

namespace Code.Scripts.Components;

public partial class AnimationHandler : AnimationPlayer
{
    /// <summary>
    /// Plays the animation of the input.
    /// </summary>
    /// <param name="input"></param>
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

    /// <summary>
    /// Plays the RESET animation
    /// </summary>
    public void PlayReset()
    {
        Play("RESET");
    }
}

