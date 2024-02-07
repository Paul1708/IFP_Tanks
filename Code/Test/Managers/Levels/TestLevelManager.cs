using GdUnit4;
using Godot;
using Managers.Level;

[TestSuite]
public class TestLevelManager
{
    ISceneRunner runner;
    LevelManager manager;
    [BeforeTest]
    public void SetUp()
    {
        runner = ISceneRunner.Load("res://Test/Managers/Levels/TestLevelManager.tscn");
        manager = runner.Scene().GetTree().GetNodesInGroup("LevelManager")[0] as LevelManager;
    }

    [TestCase]
    public void CorrectlyInitializeFirstLevel()
    {
        Node level = runner.Scene().GetChild(0);

        Assertions.AssertBool(level != null).IsTrue();
        Assertions.AssertThat(level.Name.ToString()).IsEqual("TestLevel1");
    }

    /* I give up on this  one ... 
        [TestCase]
        public async Task CorrectlyLoadNextLevel()
        {
            await Task.Run(() => manager.OnLevelComplete());

            Node level = runner.Scene().GetChild(0);

            GD.Print("Assertions");
            Assertions.AssertBool(level != null).IsTrue();
            Assertions.AssertThat(level.Name.ToString()).IsEqual("TestLevel2");
        }
    */
}