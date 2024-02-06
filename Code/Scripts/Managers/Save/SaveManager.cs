using Godot;

namespace Managers.Save;

public partial class SaveManager : Node
{
    public SaveData SaveData { get; private set; }

    [Signal]
    public delegate void OnSaveDataLoadedEventHandler(SaveData saveData);

    public override void _Ready()
    {
        if (IsSaveFileAvailable())
        {
            LoadGame();
            return;
        }

        SaveData = new SaveData();
    }


    public void SaveGame()
    {
        SaveData.PlayerCurrentHP = Manager.Instance.PlayerManager.PlayerHealthComponent.currentHP;
        SaveData.PlayerMaxHP = Manager.Instance.PlayerManager.PlayerHealthComponent.maxHP;
        SaveData.CoinCount = Manager.Instance.CoinManager.Coins;

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