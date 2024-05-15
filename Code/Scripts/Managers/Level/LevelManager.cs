using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Managers.Save;
using Shop;

namespace Managers.Level;

public partial class LevelManager : Node2D
{
    [Export]
    public WorldData[] Worlds { get; private set; }
    public int currentWorldID = 0;
    public int currentLevelID = 0;
    public Level CurrentLevelInstance { get; private set; }
    public LevelData CurrentLevelData { get; private set; }
    private LevelDisplay _levelDisplay;
    private ShopMenu _shopMenu;
    private LevelCountdown _levelCountdown;
    private LevelFailed _levelFailed;
    [Signal]
    public delegate void OnLevelChangedEventHandler();
    [Signal]
    public delegate void OnLevelChangedShowShopEventHandler();
    [Signal]
    public delegate void OnFirstLevelLoadedEventHandler();
    [Signal]
    public delegate void OnLevelResetEventHandler();
    [Signal]
    public delegate void OnLevelFailedShowDeathScreenEventHandler();


    public override void _Ready()
    {
        _levelDisplay = GetTree().GetFirstNodeInGroup("LevelDisplay") as LevelDisplay;
        _shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
        _levelCountdown = GetTree().GetFirstNodeInGroup("LevelCountdown") as LevelCountdown;
        _levelFailed = GetTree().GetFirstNodeInGroup("LevelFailed") as LevelFailed;

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
            EmitSignal(SignalName.OnLevelChanged);
            EmitSignal(SignalName.OnLevelChangedShowShop);

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
            EmitSignal(SignalName.OnLevelChanged);
            EmitSignal(SignalName.OnLevelChangedShowShop);

            return;
        }

        // If there are no levels left in the game, go back to the main menu
        GD.Print("You win!");
        CoinManager.Instance.ResetCoins();
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }

    public async void OnLevelFailed()
    {
        EmitSignal(SignalName.OnLevelFailedShowDeathScreen);
        await ToSignal(_levelFailed, "OnRetryPressed");

        // Reload the last save
        SaveManager.Instance.LoadGame(LoadingType.LOAD_GAME);
        EmitSignal(SignalName.OnLevelReset);
        EmitSignal(SignalName.OnLevelChangedShowShop);
    }

    private void SaveGame()
    {
        // Set the Level and World IDs as the last checkpoint and save the game
        SaveManager.Instance.SaveData.LastCheckpointLevelID = currentLevelID;
        SaveManager.Instance.SaveData.LastCheckpointWorldID = currentWorldID;
        SaveManager.Instance.SaveGame();
    }

    private async void OnSaveDataLoaded(SaveData saveData)
    {
        // Set the current level to saved data
        currentLevelID = saveData.LastCheckpointLevelID;
        currentWorldID = saveData.LastCheckpointWorldID;

        // Load the level and restore the level states
        await LoadLevelByID(currentWorldID, currentLevelID);

        if (currentLevelID > 0 || currentWorldID > 0) // If the game is not at the start, show the shop
        {
            await ToSignal(_shopMenu, "ready");
            EmitSignal(SignalName.OnLevelChangedShowShop);
        }
        else if (currentLevelID == 0 && currentWorldID == 0) // If the game is at the start, start the countdown without showing the shop
        {
            await ToSignal(_levelCountdown, "ready");
            EmitSignal(SignalName.OnFirstLevelLoaded);
        }

        EmitSignal(SignalName.OnLevelChanged);
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
        _levelDisplay.RenderLevelDisplay(Worlds[currentWorldID].Levels);
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

        _levelDisplay.RenderLevelDisplay(Worlds[currentWorldID].Levels);
    }
}