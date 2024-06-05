using System.Collections.Generic;
using Code.Scripts.Components;
using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using GdUnit4;
using Godot;

namespace Code.Test.Managers.Levels;

[TestSuite]
public class TestLevel
{
    Level _level;
    ISceneRunner _runner;

    [BeforeTest]
    public void Setup()
    {
        _runner = ISceneRunner.Load("res://Test/Managers/Levels/TestLevel1.tscn");

        _level = _runner.Scene().GetTree().GetNodesInGroup("Level")[0] as Level;
    }


    [TestCase]
    public void CorrectlyInitializeEnemyList()
    {
        Assertions.AssertThat(_level.Enemies.Count).IsEqual(2);
    }


    [TestCase]
    public void EnemyGetsRemovedFromListWhenItDies()
    {
        // Kill the first enemy
        Node enemy = _level.Enemies[0];
        enemy.GetNode<HealthComponent>("HealthComponent").TakeDamage(100);

        Assertions.AssertThat(_level.Enemies.Count).IsEqual(1);
    }

    [TestCase]
    public void LevelIsCompletedWhenAllEnemiesAreDead()
    {
        _level = _runner.Scene().GetTree().GetNodesInGroup("Level")[0] as Level;


        // Hook into the OnLevelComplete event
        bool isCompleted = false;
        _level.OnLevelComplete += () => { isCompleted = true; };

        // Kill all enemies, deep copy the list to avoid concurrent modification
        List<Node> enemies = new List<Node>(_level.Enemies);
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
        _level.OnLevelFailed += () => { isFailed = true; };

        // Kill the player
        PlayerManager.Instance.PlayerHealthComponent.TakeDamage(100);

        Assertions.AssertBool(isFailed).IsTrue();
    }
}