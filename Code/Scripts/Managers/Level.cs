using Godot;
using System.Linq;
using Components;
using System.Collections.Generic;
using Movement;

namespace Managers;

readonly struct Items
{
	public static readonly PackedScene Coin = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Coin.tscn");
}

public partial class Level : Node2D
{

    [Export]
    public bool IsCheckpoint { get; private set; }
    public List<Node> enemies { get; set; }

    [Signal]
    public delegate void OnLevelCompleteEventHandler();
    [Signal]
    public delegate void OnLevelFailedEventHandler();

    public override void _Ready()
    {
        // Get all enemies in the level
        enemies = GetTree().GetNodesInGroup("Enemy").ToList();

        foreach (Node2D enemy in enemies)
        {
            // Connect Signals, so OnEnemyDeath is called when an enemy dies
            enemy.GetNode<HealthComponent>("HealthComponent").OnDeath += () => OnEnemyDeath(enemy);
        }
        GetNode<Player>("Player").GetNode<HealthComponent>("HealthComponent").OnDeath += OnPlayerDeath;
    }

    public void OnEnemyDeath(Node2D enemy)
    {
        DropItem(enemy, Items.Coin);
       
        // Remove all dead enemies from the list
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            EmitSignal(SignalName.OnLevelComplete);
        }
    }

    public void OnPlayerDeath()
    {
        EmitSignal(SignalName.OnLevelFailed);
    }

    public void DropItem(Node2D position, PackedScene scene)
	{
		var item = scene.Instantiate<Node2D>();

		SetPostion(item, position);
		GetTree().GetFirstNodeInGroup("Level").CallDeferred("add_child", item);

	}
    private static void SetPostion(Node2D item, Node2D position)
	{
		item.GlobalPosition = position.GlobalPosition;
	}
}