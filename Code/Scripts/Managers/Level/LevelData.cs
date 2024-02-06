using Godot;

namespace Managers.Level;


public partial class LevelData : Resource
{
    public PackedScene LevelScene { get; set; }
    public bool IsCheckpoint { get; set; }
    public LevelSate LevelState { get; set; }

}

public enum LevelSate
{
    CURRENT,
    COMPLETED,
    LOCKED
}