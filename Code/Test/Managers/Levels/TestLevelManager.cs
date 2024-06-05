
using GdUnit4;
using Godot;

namespace Code.Test.Managers.Levels;

[TestSuite]
public class TestLevelManager
{
    ISceneRunner _runner;    
    [BeforeTest]
    public void SetUp()
    {
        _runner = ISceneRunner.Load("res://Test/Managers/Levels/TestLevelManager.tscn");
    }

    [TestCase]
    public void CorrectlyInitializeFirstLevel()
    {
        Node level = _runner.Scene().GetChild(0);

        Assertions.AssertBool(level != null).IsTrue();
        Assertions.AssertThat(level.Name.ToString()).IsEqual("TestLevel1");
    }
    
}