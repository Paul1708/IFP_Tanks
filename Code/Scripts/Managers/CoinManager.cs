using Godot;

public partial class CoinManager : Node2D
{
    // Singleton instance
    public static CoinManager Instance { get; private set; }

    [Export]
    public int Coins { get; set; } = 0;

    //Signal that will be emitted when the Coins value changes
    [Signal]
    public delegate void OnCoinChangedEventHandler(int coins);
   
    //variable to store the Coin ammount at the last checkpoint
    public int checkpointCoins;
    
    //Method to add coins to the player. If the value is less than 0, the method will return without doing anything.
    public void AddCoins(int value)
    {
        if (value < 0)
        {
            return;
        }

        SetCoins(Coins + value);

    }
    // Method to remove coins from the player If the value is less than 0, the method will return without doing anything.
    public void RemoveCoins(int value)
    {
        if (value < 0)
        {
            return;
        }

        SetCoins(Coins - value);
    }

    // Method to set the Coins to a specific value and emit the OnCoinChanged signal. If the value is less than 0, the Coins will be set to 0.
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

    // Method to reset the Coins to default values (0)
    public void ResetCoins()
    {
        Coins = 0;
        checkpointCoins = 0;
    }
    // Method to save the Coin ammount at the last checkpoint
    public void OnCheckpointSaveCoins()
    {
        checkpointCoins = Coins;
    }
    // Method to check if the player has more or equal coins to the value
    public bool CheckIfEnoughCoins(int value)
    {
        return Coins >= value;
    }
    // Method to get the Coin ammount at the last checkpoint
    public int GetCheckpointCoins()
    {
        return checkpointCoins;
    }
}

