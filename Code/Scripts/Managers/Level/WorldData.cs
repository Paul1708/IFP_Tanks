using Godot;

namespace Code.Scripts.Managers.Level;

/// <summary>
/// Data class which holds the data of a world
/// </summary>
public partial class WorldData : Resource
{
    [Export]
    public LevelData[] Levels { get; set; }

    public int LevelCount
    {
        get
        {
            return Levels.Length;
        }
    }

    public WorldData()
    {
        Levels = new LevelData[0];
    }
}