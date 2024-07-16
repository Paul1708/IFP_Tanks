using Godot;

namespace Code.Scripts.Managers.Level;

/// <summary>
/// Data class which holds the data of a level
/// </summary>
[GlobalClass]
public partial class LevelData : Resource
{
    [Export]
    public PackedScene LevelScene { get; set; }
    [Export]
    public bool IsCheckpoint { get; set; }
    [Export]
    public LevelState LevelState { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
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