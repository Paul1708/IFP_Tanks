using Godot;

namespace Managers.Save;

public partial class SaveManager : Node
{
    public static SaveManager Instance { get; private set; }
    public SaveData SaveData { get; private set; }

    [Signal]
    public delegate void OnSaveDataLoadedEventHandler(SaveData saveData);

    public override void _Ready()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            QueueFree(); // Ensures there is only one instance of SaveManager
        }

        if (IsSaveFileAvailable())
        {
            LoadGame();
            return;
        }

        SaveData = new SaveData();
    }


    public void SaveGame()
    {
        SaveData.PlayerCurrentHP = PlayerManager.Instance.PlayerHealthComponent.currentHP;
        SaveData.PlayerMaxHP = PlayerManager.Instance.PlayerHealthComponent.maxHP;
        SaveData.CoinCount = CoinManager.Instance.Coins;

        ResourceSaver.Save(SaveData, "user://savegame.tres");
        GD.Print("Data Saved!");
    }

    public void LoadGame()
    {
        SaveData = ResourceLoader.Load<SaveData>("user://savegame.tres");
        GD.Print("Data Loaded!");
        EmitSignal(SignalName.OnSaveDataLoaded, SaveData);
    }

    public bool IsSaveFileAvailable()
    {
        return ResourceLoader.Exists("user://savegame.tres");
    }

}