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

    private LevelDisplay levelDisplay;

    public override void _Ready()
    {
        levelDisplay = GetTree().GetFirstNodeInGroup("LevelDisplay") as LevelDisplay;
        SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;

        SaveManager.Instance.LoadGame();
    }
    public override void _ExitTree()
    {
        SaveManager.Instance.OnSaveDataLoaded -= OnSaveDataLoaded;
    }

    public async void OnLevelComplete()
    {
        // Was the level a checkpoint?
        bool saveGameAfterLoading = CurrentLevelData.IsCheckpoint;

        // If there are levels left in the current world, load the next level
        if (currentLevelID + 1 < Worlds[currentWorldID].LevelCount)
        {
            await LoadLevelByID(currentWorldID, currentLevelID + 1);

            // Save the game if the level was a checkpoint
            if (saveGameAfterLoading)
            {
                SaveGame();
            }
            return;
        }

        // If there are no levels left in the current world, load the next world
        if (currentWorldID + 1 < Worlds.Length)
        {
            await LoadLevelByID(currentWorldID + 1, 0);

            // Save the game if the level was a checkpoint
            if (saveGameAfterLoading)
            {
                SaveGame();
            }
            return;
        }

        // If there are no levels left in the game, go back to the main menu
        GD.Print("You win!");
        CoinManager.Instance.ResetCoins();
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }

    public void OnLevelFailed()
    {
        // Reload the last save
        SaveManager.Instance.LoadGame(LoadingType.LOAD_GAME);
    }

    private void SaveGame()
    {
        // Set the Level and World IDs as the last checkpoint and save the game
        SaveManager.Instance.SaveData.LastCheckpointLevelID = currentLevelID;
        SaveManager.Instance.SaveData.LastCheckpointWorldID = currentWorldID;
        SaveManager.Instance.SaveGame();
    }

    private void OnSaveDataLoaded(SaveData saveData)
    {
        // Set the current level to saved data
        currentLevelID = saveData.LastCheckpointLevelID;
        currentWorldID = saveData.LastCheckpointWorldID;

        // Load the level and restore the level states
        LoadLevelByID(currentWorldID, currentLevelID);
        RestoreLevelStatesAfterSave();
    }

    private void RestoreLevelStatesAfterSave()
    {
        // Set all levels before current level to COMPLETED and all levels after the current level to LOCKED
        for (int i = 0; i < Worlds.Length; i++)
        {
            for (int j = 0; j < Worlds[i].Levels.Length; j++)
            {
                if (i < currentWorldID || (i == currentWorldID && j <= currentLevelID))
                {
                    Worlds[i].Levels[j].LevelState = LevelState.COMPLETED;
                }
                else
                {
                    Worlds[i].Levels[j].LevelState = LevelState.LOCKED;
                }
            }
        }

        // Set the current level to CURRENT
        Worlds[currentWorldID].Levels[currentLevelID].LevelState = LevelState.CURRENT;
        levelDisplay.RenderLevelDisplay(Worlds[currentWorldID].Levels);
    }


    private async Task LoadLevelByID(int NewWorldID, int NewLevelID)
    {
        if (CurrentLevelInstance != null)
        {
            CurrentLevelData.LevelState = LevelState.COMPLETED;

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
        CurrentLevelData.LevelState = LevelState.CURRENT;

        levelDisplay.RenderLevelDisplay(Worlds[currentWorldID].Levels);
    }

}