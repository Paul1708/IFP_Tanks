using GdUnit4;
using Godot;
using Movement;

[TestSuite]
public class TestPlayer
{
    Player player;

    [BeforeTest]
    public void Setup()
    {
        player = new Player()
        {
            speed = 300,
        };
    }

    [TestCase(1, 0)]
    [TestCase(0, 1)]
    [TestCase(1, 1)]
    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    [TestCase(0, 0)]
    public void VelocityGetsCorrectlyCalculated(int x, int y)
    {
        // Arrange
        Vector2 input = new Vector2(x, y);
        Vector2 expected = new Vector2(300 * x, 300 * y);

        // Act
        Vector2 actual = player.CalculateVelocity(input);

        // Assert
        Assertions.AssertThat(actual).IsEqual(expected);
    }


    [AfterTest]
    public void Clear()
    {
        player.QueueFree();
    }
}