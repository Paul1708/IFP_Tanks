using Godot;
using System.Linq;
using Components;
using System.Collections.Generic;
using Items;

namespace Managers;

public partial class Level : Node2D
{
    private bool startedCoinMovement = false;
    public ShopMenu shopMenu;
    Timer coinTimer = new Timer();

    [Export]
    public bool IsCheckpoint { get; private set; }
    public List<Node> enemies { get; set; }

    [Signal]
    public delegate void OnLevelCompleteEventHandler();
    [Signal]
    public delegate void OnLevelFailedEventHandler();
    [Signal]
    public delegate void OnCoinsMovedEventHandler();


    public override void _Ready()
    {
        this.AddChild(coinTimer);
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;

        // Get all enemies in the level
        enemies = GetTree().GetNodesInGroup("Enemy").ToList();

        foreach (Node2D enemy in enemies)
        {
            // Connect Signals, so OnEnemyDeath is called when an enemy dies
            enemy.GetNode<HealthComponent>("HealthComponent").OnDeath += () => OnEnemyDeath(enemy);
        }
        Manager.Instance.PlayerManager.PlayerHealthComponent.OnDeath += OnPlayerDeath;
        //connect the coin timer signal to the move all coins to player function
        coinTimer.Timeout += MoveAllCoinsToPlayer;

    }

    public override void _ExitTree()
    {
        //Disconnect Signals
        Manager.Instance.PlayerManager.PlayerHealthComponent.OnDeath -= OnPlayerDeath;
    }

    public override void _Process(double delta)
    {
        // Check if all coins are collected
        if (GetTree().GetNodesInGroup("Coins").Count == 0 && startedCoinMovement)
        {
            //If all coins in tree are collected because of MoveAllCoinsToPlayer function then emit the signal
                EmitSignal(SignalName.OnCoinsMoved);
                startedCoinMovement = false;
        }
    }

    public void OnEnemyDeath(Node2D enemy)
    {
        PackedScene itemScene = ((Enemy)enemy).DropItemScene;
        DropItem<Node2D>(enemy, itemScene);

        // Remove all dead enemies from the list
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            //start the timer to move all coins to the player after time runs out
            coinTimer.WaitTime = 0.5;
            coinTimer.Start();
        }
    }

    public void SendOnLevelComplete()
    {
        EmitSignal(SignalName.OnLevelComplete);
    }

    public void OnPlayerDeath()
    {
        EmitSignal(SignalName.OnLevelFailed);
    }

    // Drop an item at a position
    public void DropItem<T>(Node2D position, PackedScene scene) where T : Node2D
    {
        var item = scene.Instantiate() as T;

        if (item is Coin coin)
        {
            var randomNumberGenerator = new RandomNumberGenerator();
            coin.SetCoinValue(randomNumberGenerator.RandiRange(1, 5));
        }

        SetPostion(item, position);
        GetTree().GetFirstNodeInGroup("Level").CallDeferred("add_child", item);
    }

    private static void SetPostion(Node2D item, Node2D position)
    {
        item.GlobalPosition = position.GlobalPosition;
    }

    // Move all coins to the player
    public void MoveAllCoinsToPlayer()
    {
        coinTimer.Stop();
        var tree = GetTree();
        if (tree == null)
        {
            return;
        }
        
        var coins = GetTree().GetNodesInGroup("Coins");

        foreach (Coin coin in coins)
        {
            coin.shouldMove = true;
        }
        startedCoinMovement = true;
    }
}