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
    public int lastCheckpointWorldID = 0;
    public int lastCheckpointLevelID = 0;


    public Level CurrentLevelInstance { get; private set; }
    public LevelData CurrentLevelData { get; private set; }

    private List<Vector2> playedLevelsSinceCheckpoint = new List<Vector2>();


    public override void _Ready()
    {
        SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;

        LoadLevelByID(0, 0);
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
            CoinManager.Instance.SetCheckpointCoins();
            PlayerManager.Instance.PlayerHealthComponent.HealToMax();
            playedLevelsSinceCheckpoint.Clear();
            SaveData();
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
        // Set LevelState of all played levels after checkpoint to LOCKED
        foreach (var level in playedLevelsSinceCheckpoint)
        {
            Worlds[(int)level.X].Levels[(int)level.Y].LevelState = LevelSate.LOCKED;
        }

        CoinManager.Instance.SetCoins(CoinManager.Instance.checkpointCoins);
        PlayerManager.Instance.PlayerHealthComponent.HealToMax();
        LoadLevelByID(lastCheckpointWorldID, lastCheckpointLevelID);
    }

    private void SaveData()
    {
        lastCheckpointLevelID = currentLevelID;
        lastCheckpointWorldID = currentWorldID;
        SaveManager.Instance.SaveData.LastCheckpointLevelID = lastCheckpointLevelID;
        SaveManager.Instance.SaveData.LastCheckpointWorldID = lastCheckpointWorldID;

        SaveManager.Instance.SaveGame();
    }

    private void OnSaveDataLoaded(SaveData saveData)
    {
        lastCheckpointLevelID = saveData.LastCheckpointLevelID;
        lastCheckpointWorldID = saveData.LastCheckpointWorldID;

        LoadLevelByID(saveData.LastCheckpointWorldID, saveData.LastCheckpointLevelID);
    }


    private async Task LoadLevelByID(int WorldID, int LevelID)
    {
        if (CurrentLevelInstance != null)
        {
            CurrentLevelData.LevelState = LevelSate.COMPLETED;

            // Disconnect signals
            CurrentLevelInstance.OnLevelComplete -= OnLevelComplete;
            CurrentLevelInstance.OnLevelFailed -= OnLevelFailed;

            // Unload current level
            CurrentLevelInstance.QueueFree();
            await ToSignal(CurrentLevelInstance, "tree_exited");
        }

        // Load new level
        CurrentLevelData = Worlds[WorldID].Levels[LevelID];
        CurrentLevelInstance = CurrentLevelData.LevelScene.Instantiate<Level>();
        AddChild(CurrentLevelInstance);
        GD.Print(CurrentLevelInstance);

        // Set the level IDs as the current level
        currentLevelID = LevelID;
        currentWorldID = WorldID;

        // Connect signals
        CurrentLevelInstance.OnLevelComplete += OnLevelComplete;
        CurrentLevelInstance.OnLevelFailed += OnLevelFailed;

        // Set the level state and add it to the list of played levels since the last checkpoint
        CurrentLevelData.LevelState = LevelSate.CURRENT;
        playedLevelsSinceCheckpoint.Add(new Vector2(currentWorldID, currentLevelID));
    }











    /*
    [Export]
    public PackedScene[] Levels { get; private set; }
    public int CurrentLevelID { get; private set; }
    public int LastCheckpointID { get; private set; }

    public Level CurrentLevel { get; private set; }

    public override void _Ready()
    {
        CurrentLevelID = 0;
        LastCheckpointID = 0;
        // Manager.Instance.PlayerManager.PlayerHealthComponent.HealToMax();

        LoadLevelByID(CurrentLevelID);
    }

    public void OnLevelComplete()
    {
        int nextLevelID = CurrentLevelID + 1;

        if (nextLevelID >= Levels.Length)
        {
            GD.Print("You win! \nCoins reseted!");
            Manager.Instance.CoinManager.ResetCoins();
            GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
            return;
        }

        LoadLevelByID(nextLevelID);
    }

    public void OnLevelFailed()
    {
        Manager.Instance.CoinManager.SetCoins(Manager.Instance.CoinManager.checkpointCoins);
        LoadLevelByID(LastCheckpointID);
    }



    private async Task LoadLevelByID(int LevelID)
    {
        if (CurrentLevel != null)
        {
            // Disconnect signals
            CurrentLevel.OnLevelComplete -= OnLevelComplete;
            CurrentLevel.OnLevelFailed -= OnLevelFailed;

            // Unload current level
            CurrentLevel.QueueFree();
            await ToSignal(CurrentLevel, "tree_exited");
        }

        // Load new level
        CurrentLevel = Levels[LevelID].Instantiate<Level>();
        AddChild(CurrentLevel);

        // Set the level as the current level
        CurrentLevelID = LevelID;
        CurrentLevel.OnLevelComplete += OnLevelComplete;
        CurrentLevel.OnLevelFailed += OnLevelFailed;

        // Set Checkpoint
        if (CurrentLevel.IsCheckpoint)
        {
            Manager.Instance.CoinManager.OnCheckpointSaveCoins();
            Manager.Instance.PlayerManager.PlayerHealthComponent.HealToMax();
            LastCheckpointID = LevelID;
        }
    }

    // TODO: For debug purposes, remove later
    public override async void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Debug"))
        {
            OnLevelComplete();
        }
    }
    */
}