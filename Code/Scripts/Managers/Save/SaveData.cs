using Godot;

namespace Managers.Save;

public partial class SaveData : Resource
{
    public int LastCheckpointLevelID { get; set; }
    public int LastCheckpointWorldID { get; set; }
    public int CoinCount { get; set; }
    public int PlayerMaxHP { get; set; }
    public int PlayerCurrentHP { get; set; }
}