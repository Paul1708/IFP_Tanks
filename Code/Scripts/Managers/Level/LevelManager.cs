using System.Threading.Tasks;
using Code.Scripts.Audio;
using Godot;
using Code.Scripts.Managers.Save;
using Code.Scripts.UI;
using Code.Scripts.UI.Shop;

namespace Code.Scripts.Managers.Level;

/// <summary>
/// Manages the levels and worlds of the game, loading and saving the game, and handling level completion and failure.
/// </summary>
public partial class LevelManager : Node2D
{
    [Export]
    public WorldData[] Worlds { get; private set; }
    public int CurrentWorldId;
    public int CurrentLevelId;
    public Level CurrentLevelInstance { get; private set; }
    public LevelData CurrentLevelData { get; private set; }
    private LevelDisplay _levelDisplay;
    private ShopMenu _shopMenu;
    private LevelCountdown _levelCountdown;
    private LevelFailed _levelFailed;
    private VictoryScreen _victoryScreen;
    private MusicController _musicController;
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
    [Signal]
    public delegate void OnGameWonShowVictoryScreenEventHandler();


    public override void _Ready()
    {
        _levelDisplay = GetTree().GetFirstNodeInGroup("LevelDisplay") as LevelDisplay;
        _shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
        _levelCountdown = GetTree().GetFirstNodeInGroup("LevelCountdown") as LevelCountdown;
        _levelFailed = GetTree().GetFirstNodeInGroup("LevelFailed") as LevelFailed;
        _victoryScreen = GetTree().GetFirstNodeInGroup("VictoryScreen") as VictoryScreen;
        _musicController = GetNode<MusicController>("/root/MusicController");

        SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;

        SaveManager.Instance.LoadGame();

        _musicController.PlayWorldMusic(CurrentWorldId);
    }
    public override void _ExitTree()
    {
        SaveManager.Instance.OnSaveDataLoaded -= OnSaveDataLoaded;
    }

    /// <summary>
    /// Called when the level is completed. Loads the next level or world.
    /// </summary>
    public async void OnLevelComplete()
    {

        // Was the level a checkpoint?
        bool saveGameAfterLoading = CurrentLevelData.IsCheckpoint;


        // If there are levels left in the current world, load the next level
        if (CurrentLevelId + 1 < Worlds[CurrentWorldId].LevelCount)
        {
            await LoadLevelById(CurrentWorldId, CurrentLevelId + 1);

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
        if (CurrentWorldId + 1 < Worlds.Length)
        {
            await LoadLevelById(CurrentWorldId + 1, 0);
           
            _musicController.StopCurrentMusic();
            _musicController.PlayWorldMusic(CurrentWorldId);

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
        EmitSignal(SignalName.OnGameWonShowVictoryScreen);
        await ToSignal(_victoryScreen, "OnBacktoMainMenuPressed");
        
        CoinManager.Instance.ResetCoins();
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }

    /// <summary>
    /// Called when the level is failed. Shows the death screen and reloads the last save.
    /// </summary>
    public async void OnLevelFailed()
    {
        EmitSignal(SignalName.OnLevelFailedShowDeathScreen);
        await ToSignal(_levelFailed, "OnRetryPressed");

        // Reload the last save
        SaveManager.Instance.LoadGame(LoadingType.LoadGame);
        EmitSignal(SignalName.OnLevelReset);
        EmitSignal(SignalName.OnLevelChangedShowShop);
    }

    /// <summary>
    /// Saves the game at the current level and world
    /// </summary>
    private void SaveGame()
    {
        // Set the Level and World IDs as the last checkpoint and save the game
        SaveManager.Instance.SaveData.LastCheckpointLevelId = CurrentLevelId;
        SaveManager.Instance.SaveData.LastCheckpointWorldId = CurrentWorldId;
        SaveManager.Instance.SaveGame();
    }

    /// <summary>
    /// Called when the save data is loaded. Loads the level and restores the level states.
    /// </summary>
    /// <param name="saveData">The save data that is loaded</param>
    private async void OnSaveDataLoaded(SaveData saveData)
    {
        // Set the current level to saved data
        CurrentLevelId = saveData.LastCheckpointLevelId;
        CurrentWorldId = saveData.LastCheckpointWorldId;

        // Load the level and restore the level states
        await LoadLevelById(CurrentWorldId, CurrentLevelId);

        if (CurrentLevelId > 0 || CurrentWorldId > 0) // If the game is not at the start, show the shop
        {
            await ToSignal(_shopMenu, "ready");
            EmitSignal(SignalName.OnLevelChangedShowShop);
        }
        else if (CurrentLevelId == 0 && CurrentWorldId == 0) // If the game is at the start, start the countdown without showing the shop
        {
            await ToSignal(_levelCountdown, "ready");
            EmitSignal(SignalName.OnFirstLevelLoaded);
        }

        EmitSignal(SignalName.OnLevelChanged);
        RestoreLevelStatesAfterSave();
    }

    /// <summary>
    /// Restores the level states after the save data is loaded.
    /// </summary>
    private void RestoreLevelStatesAfterSave()
    {
        // Set all levels before current level to COMPLETED and all levels after the current level to LOCKED
        for (int i = 0; i < Worlds.Length; i++)
        {
            for (int j = 0; j < Worlds[i].Levels.Length; j++)
            {
                if (i < CurrentWorldId || (i == CurrentWorldId && j <= CurrentLevelId))
                {
                    Worlds[i].Levels[j].LevelState = LevelState.Completed;
                }
                else
                {
                    Worlds[i].Levels[j].LevelState = LevelState.Locked;
                }
            }
        }

        // Set the current level to CURRENT
        Worlds[CurrentWorldId].Levels[CurrentLevelId].LevelState = LevelState.Current;
        _levelDisplay.RenderLevelDisplay(Worlds[CurrentWorldId].Levels);
    }

    /// <summary>
    /// Loads the level by the input world and level IDs.
    /// </summary>
    /// <param name="newWorldId">The world ID of the level to load</param>
    /// <param name="newLevelId">The level ID of the level to load</param>
    /// <returns name="Task">The task that loads the level</returns>
    private async Task LoadLevelById(int newWorldId, int newLevelId)
    {
        if (CurrentLevelInstance != null)
        {
            CurrentLevelData.LevelState = LevelState.Completed;

            // Disconnect signals of old Level
            CurrentLevelInstance.OnLevelComplete -= OnLevelComplete;
            CurrentLevelInstance.OnLevelFailed -= OnLevelFailed;

            // Unload current level
            CurrentLevelInstance.QueueFree();
            await ToSignal(CurrentLevelInstance, "tree_exited");
        }

        // Load new level
        CurrentLevelData = Worlds[newWorldId].Levels[newLevelId];
        CurrentLevelInstance = CurrentLevelData.LevelScene.Instantiate<Level>();
        AddChild(CurrentLevelInstance);

        // Set the level IDs as the current level
        CurrentLevelId = newLevelId;
        CurrentWorldId = newWorldId;

        // Connect signals
        CurrentLevelInstance.OnLevelComplete += OnLevelComplete;
        CurrentLevelInstance.OnLevelFailed += OnLevelFailed;

        // Set the level state and add it to the list of played levels since the last checkpoint
        CurrentLevelData.LevelState = LevelState.Current;

        _levelDisplay.RenderLevelDisplay(Worlds[CurrentWorldId].Levels);
    }
}