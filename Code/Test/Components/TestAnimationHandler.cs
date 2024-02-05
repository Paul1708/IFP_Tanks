using Components;
using GdUnit4;
using Godot;

[TestSuite]
public class TestAnimationHandler
{
    AnimationHandler ah;

    [BeforeTest]
    public void Setup()
    {
        ah = new AnimationHandler();
    }


    [TestCase]
    public void CorrectlyPlaysNoAnimation()
    {
        ah.PlayAnimationOfInput(new Vector2(0, 0));
        Assertions.AssertString(ah.CurrentAnimation).Equals("");
    }

    [TestCase]
    public void CorrectlyPlaysForwardAnimation()
    {
        ah.PlayAnimationOfInput(new Vector2(0, 1));
        Assertions.AssertString(ah.CurrentAnimation).Equals("Forward");
    }

    [TestCase]
    public void CorrectlyPlaysBackwardAnimation()
    {
        ah.PlayAnimationOfInput(new Vector2(0, -1));
        Assertions.AssertString(ah.CurrentAnimation).Equals("Backward");
    }

    [TestCase]
    public void CorrectlyPlaysLeftAnimation()
    {
        ah.PlayAnimationOfInput(new Vector2(-1, 0));
        Assertions.AssertString(ah.CurrentAnimation).Equals("Left");
    }

    [TestCase]
    public void CorrectlyPlaysRightAnimation()
    {
        ah.PlayAnimationOfInput(new Vector2(1, 0));
        Assertions.AssertString(ah.CurrentAnimation).Equals("Right");
    }

    [TestCase(1, 1)]
    [TestCase(-1, 1)]
    [TestCase(0.5f, 1)]
    [TestCase(-0.5f, 1)]
    public void CorrectlyPlaysForwardAnimationWhenMovingDiagonally(float x, float y)
    {
        ah.PlayAnimationOfInput(new Vector2(x, y));
        Assertions.AssertString(ah.CurrentAnimation).Equals("Forward");
    }

    [TestCase(1, -1)]
    [TestCase(-1, -1)]
    [TestCase(0.5f, -1)]
    [TestCase(-0.5f, -1)]
    public void CorrectlyPlaysBackwardAnimationWhenMovingDiagonally(float x, float y)
    {
        ah.PlayAnimationOfInput(new Vector2(x, y));
        Assertions.AssertString(ah.CurrentAnimation).Equals("Backward");
    }

    [AfterTest]
    public void TearDown()
    {
        ah.Free();
    }
}