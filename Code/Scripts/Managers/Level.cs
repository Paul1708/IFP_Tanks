using Godot;
using System.Linq;
using Components;
using System.Collections.Generic;
using Movement;

namespace Managers;

public partial class Level : Node2D
{

    [Export]
    public bool IsCheckpoint { get; private set; }
    private List<Node> enemyList { get; set; }

    [Signal]
    public delegate void OnLevelCompleteEventHandler();
    [Signal]
    public delegate void OnLevelFailedEventHandler();

    public override void _Ready()
    {
        // Get all enemies in the level
        enemyList = GetTree().GetNodesInGroup("Enemy").ToList();

        foreach (Node enemy in enemyList)
        {
            // Connect Signals, so OnEnemyDeath is called when an enemy dies
            enemy.GetNode<HealthComponent>("HealthComponent").OnDeath += () => OnEnemyDeath(enemy);
        }


        GetNode<Player>("Player").GetNode<HealthComponent>("HealthComponent").OnDeath += OnPlayerDeath;
    }


    public void OnEnemyDeath(Node enemy)
    {
        enemyList.Remove(enemy);

        if (enemyList.Count == 0)
        {
            EmitSignal(SignalName.OnLevelComplete);
        }
    }

    public void OnPlayerDeath()
    {
        EmitSignal(SignalName.OnLevelFailed);
    }


}