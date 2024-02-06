using Godot;

namespace Managers.Level;

public partial class WorldData : Resource
{
    public LevelData[] Levels { get; set; }

    public int LevelCount
    {
        get
        {
            return Levels.Length;
        }
    }

}