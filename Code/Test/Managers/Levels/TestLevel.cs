using System.Collections.Generic;
using Components;
using GdUnit4;
using Godot;
using Managers;
using Movement;

[TestSuite]
public class TestLevel
{
    Level level;
    ISceneRunner runner;

    [BeforeTest]
    public void Setup()
    {
        runner = ISceneRunner.Load("res://Test/Managers/Levels/TestLevel1.tscn");

        level = runner.Scene().GetTree().GetNodesInGroup("Level")[0] as Level;
    }


    [TestCase]
    public void CorrectlyInitializeEnemyList()
    {
        Assertions.AssertThat(level.enemies.Count).IsEqual(2);
    }


    [TestCase]
    public void EnemyGetsRemovedFromListWhenItDies()
    {
        // Kill the first enemy
        Node enemy = level.enemies[0];
        enemy.GetNode<HealthComponent>("HealthComponent").TakeDamage(100);

        Assertions.AssertThat(level.enemies.Count).IsEqual(1);
    }

    [TestCase]
    public void LevelIsCompletedWhenAllEnemiesAreDead()
    {
        level = runner.Scene().GetTree().GetNodesInGroup("Level")[0] as Level;


        // Hook into the OnLevelComplete event
        bool isCompleted = false;
        level.OnLevelComplete += () => { isCompleted = true; };

        // Kill all enemies, deep copy the list to avoid concurrent modification
        List<Node> enemies = new List<Node>(level.enemies);
        foreach (Node enemy in enemies)
        {
            enemy.GetNode<HealthComponent>("HealthComponent").TakeDamage(100);
        }

        Assertions.AssertBool(isCompleted).IsTrue();
    }

    [TestCase]
    public void LevelIsFailedWhenPlayerDies()
    {
        // Hook into the OnLevelFailed event
        bool isFailed = false;
        level.OnLevelFailed += () => { isFailed = true; };

        // Kill the player
        Manager.Instance.PlayerManager.PlayerHealthComponent.TakeDamage(100);

        Assertions.AssertBool(isFailed).IsTrue();
    }
}