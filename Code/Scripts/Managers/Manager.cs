using Godot;
using System;

public partial class Manager : Node2D
{
    // Singleton instance
    public static Manager Instance { get; private set; }
    public CoinManager CoinManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            QueueFree(); // Ensures there is only one instance of GameManager
        }

        Instance.CoinManager = GetNode<CoinManager>("CoinManager");
        Instance.PlayerManager = GetNode<PlayerManager>("PlayerManager");
    }

    //gets all the stats from the last checkpoint and loads them
    public void LoadCheckpointStats()
    {
        //TODO: Health and other stats
        CoinManager.SetCoins(CoinManager.checkpointCoins);
    }
}

