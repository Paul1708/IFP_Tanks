using Godot;
using System.Linq;
using Components;
using System.Collections.Generic;

namespace Managers;

public partial class Level : Node2D
{

    [Export]
    public bool IsCheckpoint { get; private set; }
    private List<Node2D> enemies { get; set; }

    [Signal]
    public delegate void OnLevelCompleteEventHandler();
    [Signal]
    public delegate void OnLevelFailedEventHandler();

    public override void _Ready()
    {
        // Get all enemies in the level
        enemies = new List<Node2D>();
        foreach (Node2D enemy in GetTree().GetNodesInGroup("Enemy"))
        {
            GD.Print("Gegner" + enemy);
            enemies.Append(enemy);

            // Connect Signals, so OnEnemyDeath is called when an enemy dies
            enemy.GetNode<HealthComponent>("HealthComponent").OnDeath += OnEnemyDeath;
        }


    }

    // TODO: Test this shit
    public void OnEnemyDeath()
    {
        // Remove all dead enemies from the list
        //enemies = enemies.Where((enemy) => enemy.IsQueuedForDeletion()).ToList();
        if (enemies.Count == 0)
        {
            EmitSignal(SignalName.OnLevelComplete);
        }
    }


}