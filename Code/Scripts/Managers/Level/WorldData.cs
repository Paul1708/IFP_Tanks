using Godot;

namespace Code.Scripts.Managers.Level;

/// <summary>
/// Data class which holds the data of a world
/// </summary>
public partial class WorldData : Resource
{
    [Export]
    public LevelData[] Levels { get; set; }

    /// <summary>
    /// Returns the number of levels in the world
    /// </summary>
    public int LevelCount
    {
        get
        {
            return Levels.Length;
        }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public WorldData()
    {
        Levels = new LevelData[0];
    }
}