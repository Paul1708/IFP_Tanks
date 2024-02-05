using Godot;

public partial class CoinManager : Node2D
{
    // Singleton instance
    public static CoinManager Instance { get; private set; }

    // Game state variables
    [Export]
    public int Coins { get; set; } = 0;

    [Signal]
    public delegate void OnCoinChangedEventHandler(int coins);
    public int checkpointCoins;

    public void AddCoins(int value)
    {
        if (value < 0)
        {
            return;
        }

        SetCoins(Coins + value);
    }

    public void RemoveCoins(int value)
    {
        if (value < 0)
        {
            return;
        }

        SetCoins(Coins - value);
    }

    // Method to set the Coins
    public void SetCoins(int value)
    {
        if (value < 0)
        {
            Coins = 0;
            return;
        }

        Coins = value;
        EmitSignal(SignalName.OnCoinChanged, Coins);
    }

    // Method to reset the Coins
    public void ResetCoins()
    {
        Coins = 0;
        checkpointCoins = 0;
    }

    public void OnCheckpointSaveCoins()
    {
        checkpointCoins = Coins;
    }

    public bool CheckIfEnoughCoins(int value)
    {
        return Coins >= value;
    }

    public int GetCheckpointCoins()
    {
        return checkpointCoins;
    }
}

