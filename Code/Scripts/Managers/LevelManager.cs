using System.Threading.Tasks;
using Godot;
using Managers;

public partial class LevelManager : Node2D
{

    [Export]
    public PackedScene[] Levels { get; private set; }
    public int CurrentLevelID { get; private set; }
    public int LastCheckpointID { get; private set; }

    public Level CurrentLevel { get; private set; }

    public override async void _Ready()
    {
        CurrentLevelID = 0;
        LastCheckpointID = 0;
        await LoadLevelByID(CurrentLevelID);
    }

    public async Task OnLevelComplete()
    {
        // Remove the current level and wait until it's done bfore calling LoadNextLevel
        CurrentLevel.QueueFree();

        if (CurrentLevel.IsInsideTree())
        {
            await ToSignal(CurrentLevel, "tree_exited");
        }

        await LoadNextLevel();
    }

    public async Task OnLevelFailed()
    {
        // Remove the current level and wait until it's done before calling LoadNextLevel
        CurrentLevel.QueueFree();
        await ToSignal(CurrentLevel, "tree_exited");

        await LoadLastCheckpoint();
    }

    private async Task LoadNextLevel()
    {
        int nextLevelID = CurrentLevelID + 1;
        if (nextLevelID < Levels.Length)
        {
            // Still level left
            await LoadLevelByID(nextLevelID);
        }
        else
        {
            GD.Print("You win!");
            GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
        }
    }

    private async Task LoadLastCheckpoint()
    {
        await LoadLevelByID(LastCheckpointID);
    }

    private async Task LoadLevelByID(int levelID)
    {
        // Load the level
        CurrentLevel = Levels[levelID].Instantiate<Level>();
        AddChild(CurrentLevel);

        if (CurrentLevel.IsNodeReady() == false)
        {
            await ToSignal(CurrentLevel, "ready");
        }

        // Set the level as the current level
        CurrentLevelID = levelID;
        CurrentLevel.OnLevelComplete += async () => await OnLevelComplete();
        CurrentLevel.OnLevelFailed += async () => await OnLevelFailed(); ;

        // Set Checkpoint
        if (CurrentLevel.IsCheckpoint)
        {
            LastCheckpointID = levelID;
        }
    }

    // TODO: For debug purposes, remove later
    public override async void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Debug"))
        {
            await OnLevelComplete();
        }
    }
}