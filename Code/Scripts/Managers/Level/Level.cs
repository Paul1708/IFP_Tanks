using Godot;
using System.Linq;
using Code.Scripts.Components;
using System.Collections.Generic;
using Code.Scripts.Enemies;
using Code.Scripts.Environment;
using Code.Scripts.UI.Shop;


namespace Code.Scripts.Managers.Level;

public partial class Level : Node2D
{
    private bool _startedCoinMovement;
    private Timer _coinTimer = new ();
    public ShopMenu ShopMenu;
    public List<Node> Enemies { get; set; }
    [Signal]
    public delegate void OnLevelCompleteEventHandler();
    [Signal]
    public delegate void OnLevelFailedEventHandler();
    [Signal]
    public delegate void OnCoinsMovedEventHandler();


    public override void _Ready()
    {
        this.AddChild(_coinTimer);
        ShopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;

        // Get all enemies in the level
        Enemies = GetTree().GetNodesInGroup("Enemy").ToList();

        foreach (Node enemy in Enemies)
        {
            // Connect Signals, so OnEnemyDeath is called when an enemy dies
            enemy.GetNode<HealthComponent>("HealthComponent").OnDeath += () => OnEnemyDeath(enemy as Node2D);
        }
        PlayerManager.Instance.PlayerHealthComponent.OnDeath += OnPlayerDeath;
        //connect the coin timer signal to the move all coins to player function
        _coinTimer.Timeout += MoveAllCoinsToPlayer;
        OnCoinsMoved += SendOnLevelComplete;
    }

    public override void _ExitTree()
    {
        //Disconnect Signals
        PlayerManager.Instance.PlayerHealthComponent.OnDeath -= OnPlayerDeath;
    }

    public override void _Process(double delta)
    {
        // Check if all coins are collected
        if (GetTree().GetNodesInGroup("Coins").Count == 0 && _startedCoinMovement)
        {
            //If all coins in tree are collected because of MoveAllCoinsToPlayer function then emit the signal
            EmitSignal(SignalName.OnCoinsMoved);
            _startedCoinMovement = false;
        }
    }

    public void OnEnemyDeath(Node2D enemy)
    {
        PackedScene itemScene = ((Enemy)enemy).DropItemScene;
        DropItem<Node2D>(enemy, itemScene);

        // Remove all dead enemies from the list
        Enemies.Remove(enemy);
        if (Enemies.Count == 0)
        {
            //start the timer to move all coins to the player after time runs out
            _coinTimer.WaitTime = 0.5;
            _coinTimer.Start();
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

    /// <summary>
    /// Drops/Instantiates an item scene at the given nodes position in the scene. The coin value is set randomly if the item is a coin.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="position"></param>
    /// <param name="scene"></param>
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

    /// <summary>
    /// Sets the position of the item to the position of the given node
    /// </summary>
    /// <param name="item"></param>
    /// <param name="position"></param>
    private static void SetPostion(Node2D item, Node2D position)
    {
        item.GlobalPosition = position.GlobalPosition;
    }

    /// <summary>
    /// Move all coins to the player by setting the shouldMove property to true
    /// </summary>
    public void MoveAllCoinsToPlayer()
    {
        _coinTimer.Stop();
        var tree = GetTree();
        if (tree == null)
        {
            return;
        }

        var coins = GetTree().GetNodesInGroup("Coins");

        foreach (Node coin in coins)
        {
            if(coin is Coin c)
                c.ShouldMove = true;
        }
        _startedCoinMovement = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Debug"))
        {
            EmitSignal(SignalName.OnCoinsMoved);
            CoinManager.Instance.AddCoins(10);
        }
    }
}