using Godot;

namespace Managers.Level;

[GlobalClass]
public partial class LevelData : Resource
{
    [Export]
    public PackedScene LevelScene { get; set; }
    [Export]
    public bool IsCheckpoint { get; set; }
    [Export]
    public LevelSate LevelState { get; set; }

    public LevelData()
    {
        LevelScene = null;
        IsCheckpoint = false;
        LevelState = LevelSate.LOCKED;
    }
}

public enum LevelSate
{
    CURRENT,
    COMPLETED,
    LOCKED
}