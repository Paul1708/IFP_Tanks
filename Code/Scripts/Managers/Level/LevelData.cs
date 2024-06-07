using Godot;

namespace Code.Scripts.Managers.Level;

[GlobalClass]
public partial class LevelData : Resource
{
    [Export]
    public PackedScene LevelScene { get; set; }
    [Export]
    public bool IsCheckpoint { get; set; }
    [Export]
    public LevelState LevelState { get; set; }

    public LevelData()
    {
        LevelScene = null;
        IsCheckpoint = false;
        LevelState = LevelState.Locked;
    }
}

public enum LevelState
{
    Current,
    Completed,
    Locked
}