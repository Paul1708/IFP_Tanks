using Godot;

namespace Managers.Level;

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