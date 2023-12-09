using GdUnit4;
using Scripts;

[TestSuite]
public partial class TestAdd : GdUnit4MonoApi
{
    //PLease work
    private HelloWorld _helloWorld;

    [Before]
    public void Setup()
    {
        _helloWorld = new HelloWorld();
    }

    [TestCase]
    public void ProperlyAddNumbers()
    {
        int result = _helloWorld.Add(2, 3);
        Assertions.AssertThat(result).IsEqual(5);
    }

    [TestCase]
    public void SubstractTwoNumbers()
    {
        int result = _helloWorld.Add(35, -13);
        Assertions.AssertThat(result).IsEqual(22);
    }

    [After]
    public void Finish()
    {
        _helloWorld.Free();
    }
}