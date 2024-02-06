using Godot;

namespace Managers.Save;

public partial class SaveManager : Node
{
    public SaveData SaveData { get; private set; }

    [Signal]
    public delegate void OnSaveDataLoadedEventHandler(SaveData saveData);

    public override void _Ready()
    {
        if (ResourceLoader.Exists("user://savegame.tres"))
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

        ResourceSaver.Save(SaveData, "user://savegame.tres");
    }

    public void LoadGame()
    {
        SaveData = ResourceLoader.Load<SaveData>("user://savegame.tres");
        EmitSignal(SignalName.OnSaveDataLoaded, SaveData);
    }

}