using System.ComponentModel.DataAnnotations;
using Godot;

namespace Managers.Save;

public partial class SaveManager : Node
{
    public static SaveManager Instance { get; private set; }
    public SaveData SaveData { get; private set; }
    public LoadingType LoadingType { get; set; } = LoadingType.NONE;

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

        SaveData = new SaveData();
    }


    public void SaveGame()
    {
        SaveData.PlayerCurrentHP = PlayerManager.Instance.PlayerHealthComponent.currentHP;
        SaveData.PlayerMaxHP = PlayerManager.Instance.PlayerHealthComponent.maxHP;
        SaveData.CoinCount = CoinManager.Instance.Coins;

        ResourceSaver.Save(SaveData, "user://savegame.tres");
        GD.Print("Game Saved");
    }

    public void LoadGame(LoadingType loadingType)
    {
        LoadingType = loadingType;
        LoadGame();
    }

    public void LoadGame()
    {

        if (LoadingType == LoadingType.NEW_GAME)
        {
            SaveData = new SaveData();
            ResourceSaver.Save(SaveData, "user://savegame.tres");
            GD.Print("New Game loaded");
        }
        else if (LoadingType == LoadingType.LOAD_GAME)
        {
            SaveData = ResourceLoader.Load<SaveData>("user://savegame.tres");
            GD.Print("Game loaded from save file");
        }
        else
        {
            throw new ValidationException("LoadingType is not set. Please set the LoadingType before calling LoadGame()");
        }

        EmitSignal(SignalName.OnSaveDataLoaded, SaveData);
        LoadingType = LoadingType.NONE;
    }

    public bool IsSaveFileAvailable()
    {
        return ResourceLoader.Exists("user://savegame.tres");
    }
}

public enum LoadingType
{
    NEW_GAME,
    LOAD_GAME,
    NONE
}