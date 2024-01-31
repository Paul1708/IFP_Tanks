using Godot;
using System;

public partial class GameManager : Node2D
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Game state variables
    [Export] public int Coins { get; set; } = 0;
    public int checkpointCoins;

    // Called when the node enters the scene tree for the first time
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
    }
    // Method to set the Coins
    public void SetCoins(int value)
    {
        if (value < 0)
        {
            Coins = 0;
        }
        else
        {
            Coins = value;
        }
    }

    // Method to reset the Coins
    public void ResetCoins()
    {
        Coins = 0;
        checkpointCoins = 0;
    }

    // Method to get the Coins
    public int GetCoins()
    {
        return Coins;
    }

    public void OnCheckpointSaveCoins()
    {
        checkpointCoins = Coins;
    }
}

