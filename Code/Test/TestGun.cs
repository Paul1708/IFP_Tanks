using GdUnit4;
using Godot;

[TestSuite]
public class TestGun
{
    Gun gun;

    [BeforeTest]
    public void Setup()
    {
        gun = new Gun();
    }


    [TestCase(1)]
    [TestCase(10)]
    [TestCase(0)]
    [TestCase(-1)]
    public void FireRateGetsCorrectlyInitialized(int bps)
    {
        gun.bulletsPerSecond = bps;


        if (bps <= 0)
        {
            try
            {
                gun._Ready();

                //we should never get here
                Assertions.AssertThat(true).IsFalse();
            }
            catch (System.Exception)
            {
                Assertions.AssertThat(true).IsTrue();
            }
        }
        else
        {
            gun._Ready();

            float expected = 1f / bps;
            float actual = gun.timeBetweenShots;

            Assertions.AssertThat(actual).IsEqual(expected);
        }
        return;
    }

    [AfterTest]
    public void Clear()
    {
        gun.QueueFree();
    }
}
