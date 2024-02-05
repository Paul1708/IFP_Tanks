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
}