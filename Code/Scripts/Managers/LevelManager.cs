using Godot;
using Managers;


public partial class LevelManager : Node2D
{

    [Export]
    public PackedScene[] Levels { get; private set; }
    public int CurrentLevelID { get; private set; }
    public int LastCheckpointID { get; private set; }

    public Level CurrentLevel { get; private set; }

    public override void _Ready()
    {
        CurrentLevelID = 0;
        LastCheckpointID = 0;
        LoadLevelByID(CurrentLevelID);
    }

    public void OnLevelComplete()
    {
        // Remove the current level and wait until it's done bfore calling LoadNextLevel
        CurrentLevel.QueueFree();
        CurrentLevel.TreeExited += LoadNextLevel;
    }


    private void LoadNextLevel()
    {
        int nextLevelID = CurrentLevelID + 1;
        if (nextLevelID < Levels.Length)
        {
            // Still level left
            LoadLevelByID(nextLevelID);
        }
        else
        {
            GD.Print("You win!");
            GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
        }
    }

    public void OnLevelFailed()
    {
        // Remove the current level and wait until it's done before calling LoadNextLevel
        CurrentLevel.QueueFree();
        CurrentLevel.TreeExited += LoadLastCheckpoint;
    }

    public void LoadLastCheckpoint()
    {
        LoadLevelByID(LastCheckpointID);
    }

    private void LoadLevelByID(int levelID)
    {
        // Load the level
        CurrentLevel = Levels[levelID].Instantiate<Level>();
        AddChild(CurrentLevel);

        // Set the level as the current level
        CurrentLevelID = levelID;
        CurrentLevel.OnLevelComplete += OnLevelComplete;

        // Set Checkpoint
        if (CurrentLevel.IsCheckpoint)
        {
            LastCheckpointID = levelID;
        }
    }

    // TODO: For debug purposes, remove later
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Debug"))
        {
            OnLevelComplete();
        }
    }
}