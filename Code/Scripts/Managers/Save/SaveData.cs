using System.Collections.Generic;
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
    
    /// <summary>
    ///The first index is the stat index and the second index is the price or quantity.
    ///E.g. [0][0] to adress the price and [0][1] to adress the quantity of the first stat.
    /// </summary>
    [Export]
    public Godot.Collections.Array<Godot.Collections.Array> Stats { get; set; }
    /// <summary>
    ///The first index is the weapon index and the second index is the unlocked or equiped.
    ///E.g. [0][0] to adress the unlocked and [0][1] to adress the equiped of the first weapon.
    /// </summary>
    [Export]
    public Godot.Collections.Array<Godot.Collections.Array> Weapons { get; set; }

    public SaveData()
    {
        Stats = new Godot.Collections.Array<Godot.Collections.Array>{
            new() {0, 0},
            new() {0, 0},
            new() {0, 0},
            new() {0, 0}
        };
        Weapons = new Godot.Collections.Array<Godot.Collections.Array>{
            new() {true, true},
            new() {false, false},
            new() {false, false},
            new() {false, false}
        };
        LastCheckpointLevelID = 0;
        LastCheckpointWorldID = 0;
        CoinCount = 0;
        PlayerMaxHP = 100;
        PlayerCurrentHP = 100;
    }
}