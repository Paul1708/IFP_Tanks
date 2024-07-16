using Godot;
using Code.Scripts.Managers.Save;

namespace Code.Scripts.Managers;

/// <summary>
/// Manager class that handles the coins of the player
/// </summary>
public partial class CoinManager : Node2D
{
    public static CoinManager Instance { get; private set; }
    [Export]
    public int Coins { get; set; }
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

    /// <summary>
    /// Method that will be called when the SaveData is loaded. It will set the Coins to the value of the SaveData
    /// </summary>
    /// <param name="saveData">The SaveData that was loaded</param>
    public void OnSaveDataLoaded(SaveData saveData)
    {
        SetCoins(saveData.CoinCount);
    }

    /// <summary>
    /// Method to add coins to the player. If the value is less than 0, the method will return without doing anything.
    /// </summary>
    /// <param name="value">The value to add to the coins</param>
    public void AddCoins(int value)
    {
        if (value < 0)
        {
            return;
        }

        SetCoins(Coins + value);

    }
    /// <summary>
    /// Method to remove coins from the player If the value is less than 0, the method will return without doing anything.
    /// </summary>
    /// <param name="value">The value to remove from the coins</param>
    public void RemoveCoins(int value)
    {
        if (value < 0)
        {
            return;
        }

        SetCoins(Coins - value);
    }

    /// <summary>
    /// Method to set the Coins to a specific value and emit the OnCoinChanged signal. If the value is less than 0, the Coins will be set to 0.
    /// </summary>
    /// <param name="value">The value to set the Coins to</param>
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

    /// <summary>
    /// Method to reset the Coins to default values (0)
    /// </summary>
    public void ResetCoins()
    {
        Coins = 0;
    }

    /// <summary>
    /// Method to check if the player has more or equal coins to the value
    /// </summary>
    /// <param name="value">The value to check if the player has enough coins</param>
    /// <returns>True if the player has enough coins, false otherwise</returns>
    public bool CheckIfEnoughCoins(int value)
    {
        return Coins >= value;
    }
}

