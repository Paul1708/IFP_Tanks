
using Code.Scripts.Components;
using GdUnit4;
using Godot;

namespace Code.Test.Components;

[TestSuite]
public class TestAnimationHandler
{
    AnimationHandler _ah;

    [BeforeTest]
    public void Setup()
    {
        _ah = new AnimationHandler();
    }


    [TestCase]
    public void CorrectlyPlaysNoAnimation()
    {
        _ah.PlayAnimationOfInput(new Vector2(0, 0));
        Assertions.AssertString(_ah.CurrentAnimation).Equals("");
    }

    [TestCase]
    public void CorrectlyPlaysForwardAnimation()
    {
        _ah.PlayAnimationOfInput(new Vector2(0, 1));
        Assertions.AssertString(_ah.CurrentAnimation).Equals("Forward");
    }

    [TestCase]
    public void CorrectlyPlaysBackwardAnimation()
    {
        _ah.PlayAnimationOfInput(new Vector2(0, -1));
        Assertions.AssertString(_ah.CurrentAnimation).Equals("Backward");
    }

    [TestCase]
    public void CorrectlyPlaysLeftAnimation()
    {
        _ah.PlayAnimationOfInput(new Vector2(-1, 0));
        Assertions.AssertString(_ah.CurrentAnimation).Equals("Left");
    }

    [TestCase]
    public void CorrectlyPlaysRightAnimation()
    {
        _ah.PlayAnimationOfInput(new Vector2(1, 0));
        Assertions.AssertString(_ah.CurrentAnimation).Equals("Right");
    }

    [TestCase(1, 1)]
    [TestCase(-1, 1)]
    [TestCase(0.5f, 1)]
    [TestCase(-0.5f, 1)]
    public void CorrectlyPlaysForwardAnimationWhenMovingDiagonally(float x, float y)
    {
        _ah.PlayAnimationOfInput(new Vector2(x, y));
        Assertions.AssertString(_ah.CurrentAnimation).Equals("Forward");
    }

    [TestCase(1, -1)]
    [TestCase(-1, -1)]
    [TestCase(0.5f, -1)]
    [TestCase(-0.5f, -1)]
    public void CorrectlyPlaysBackwardAnimationWhenMovingDiagonally(float x, float y)
    {
        _ah.PlayAnimationOfInput(new Vector2(x, y));
        Assertions.AssertString(_ah.CurrentAnimation).Equals("Backward");
    }

    [AfterTest]
    public void TearDown()
    {
        _ah.Free();
    }
}