using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Managers.Save;

namespace Managers.Level;

public partial class LevelManager : Node2D
{
    [Export]
    public WorldData[] Worlds { get; private set; }

    public int currentWorldID = 0;
    public int currentLevelID = 0;

    public Level CurrentLevelInstance { get; private set; }
    public LevelData CurrentLevelData { get; private set; }

    public override void _Ready()
    {
        SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;

        SaveManager.Instance.LoadGame();
    }
    public override void _ExitTree()
    {
        SaveManager.Instance.OnSaveDataLoaded -= OnSaveDataLoaded;
    }

    public void OnLevelComplete()
    {
        // Set Checkpoint
        if (CurrentLevelData.IsCheckpoint)
        {
            SaveGame();
        }

        // If there are levels left in the current world, load the next level
        if (currentLevelID + 1 < Worlds[currentWorldID].LevelCount)
        {
            LoadLevelByID(currentWorldID, currentLevelID + 1);
            return;
        }

        // If there are no levels left in the current world, load the next world
        if (currentWorldID + 1 < Worlds.Length)
        {
            LoadLevelByID(currentWorldID + 1, 0);
            return;
        }

        // If there are no levels left in the game, go back to the main menu
        GD.Print("You win!");
        CoinManager.Instance.ResetCoins();
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }

    public void OnLevelFailed()
    {
        SaveManager.Instance.LoadGame(LoadingType.LOAD_GAME);
    }

    private void SaveGame()
    {
        SaveManager.Instance.SaveData.LastCheckpointLevelID = currentLevelID;
        SaveManager.Instance.SaveData.LastCheckpointWorldID = currentWorldID;
        SaveManager.Instance.SaveData.Worlds = Worlds;

        SaveManager.Instance.SaveGame();
    }

    private void OnSaveDataLoaded(SaveData saveData)
    {
        currentLevelID = saveData.LastCheckpointLevelID;
        currentWorldID = saveData.LastCheckpointWorldID;

        LoadLevelByID(currentWorldID, currentLevelID);
    }


    private async Task LoadLevelByID(int NewWorldID, int NewLevelID)
    {
        if (CurrentLevelInstance != null)
        {
            CurrentLevelData.LevelState = LevelSate.COMPLETED;

            // Disconnect signals of old Level
            CurrentLevelInstance.OnLevelComplete -= OnLevelComplete;
            CurrentLevelInstance.OnLevelFailed -= OnLevelFailed;

            // Unload current level
            CurrentLevelInstance.QueueFree();
            await ToSignal(CurrentLevelInstance, "tree_exited");
        }

        // Load new level
        CurrentLevelData = Worlds[NewWorldID].Levels[NewLevelID];
        CurrentLevelInstance = CurrentLevelData.LevelScene.Instantiate<Level>();
        AddChild(CurrentLevelInstance);

        // Set the level IDs as the current level
        currentLevelID = NewLevelID;
        currentWorldID = NewWorldID;

        // Connect signals
        CurrentLevelInstance.OnLevelComplete += OnLevelComplete;
        CurrentLevelInstance.OnLevelFailed += OnLevelFailed;

        // Set the level state and add it to the list of played levels since the last checkpoint
        CurrentLevelData.LevelState = LevelSate.CURRENT;
    }

}