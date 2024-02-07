using Godot;
using Managers.Level;

namespace Managers.Save;

[GlobalClass]
public partial class SaveData : Resource
{
    [Export]
    public int LastCheckpointLevelID { get; set; }
    [Export]
    public int LastCheckpointWorldID { get; set; }
    [Export]
    public int CoinCount { get; set; }
    [Export]
    public int PlayerMaxHP { get; set; }
    [Export]
    public int PlayerCurrentHP { get; set; }


    public SaveData()
    {
        LastCheckpointLevelID = 0;
        LastCheckpointWorldID = 0;
        CoinCount = 0;
        PlayerMaxHP = 100;
        PlayerCurrentHP = 100;
    }
}