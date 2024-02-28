using Godot;
using Managers.Save;

namespace Managers;

public partial class CoinManager : Node2D
{
    public static CoinManager Instance { get; private set; }
    [Export]
    public int Coins { get; set; } = 0;

    //Signal that will be emitted when the Coins value changes
    [Signal]
    public delegate void OnCoinChangedEventHandler(int coins);

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            QueueFree(); // Ensures there is only one instance of CoinManager
        }

        SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;
    }

    public override void _ExitTree()
    {
        SaveManager.Instance.OnSaveDataLoaded -= OnSaveDataLoaded;
    }

    public void OnSaveDataLoaded(SaveData saveData)
    {
        SetCoins(saveData.CoinCount);
    }

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
    }

    // Method to check if the player has more or equal coins to the value
    public bool CheckIfEnoughCoins(int value)
    {
        return Coins >= value;
    }
}

