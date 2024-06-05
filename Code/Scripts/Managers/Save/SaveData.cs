
using Godot;
using Code.Scripts.Movement;

namespace Code.Scripts.Managers.Save;

[GlobalClass]
public partial class SaveData : Resource
{
    [Export]
    public int LastCheckpointLevelId { get; set; }
    [Export]
    public int LastCheckpointWorldId { get; set; }
    [Export]
    public int CoinCount { get; set; }
    [Export]
    public float PlayerCurrentHp { get; set; }
    [Export]
    public PlayerStats PlayerStats { get; set; }


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
            new() {false, false},
            new() {false, false},
            new() {false, false},
        };
        LastCheckpointLevelId = 0;
        LastCheckpointWorldId = 0;
        CoinCount = 0;
        PlayerCurrentHp = SaveManager.Instance.InitialPlayerStats.BaseMaxHealth;
        PlayerStats = new PlayerStats()
        {
            BaseMovementSpeed = SaveManager.Instance.InitialPlayerStats.BaseMovementSpeed,
            CurrentMovementSpeed = SaveManager.Instance.InitialPlayerStats.BaseMovementSpeed,
            BaseRotationSpeed = SaveManager.Instance.InitialPlayerStats.BaseRotationSpeed,
            CurrentRotationSpeed = SaveManager.Instance.InitialPlayerStats.BaseRotationSpeed,
            BaseDamageModifier = SaveManager.Instance.InitialPlayerStats.BaseDamageModifier,
            CurrentDamageModifier = SaveManager.Instance.InitialPlayerStats.BaseDamageModifier,
            BaseMaxHealth = SaveManager.Instance.InitialPlayerStats.BaseMaxHealth,
            CurrentMaxHealth = SaveManager.Instance.InitialPlayerStats.BaseMaxHealth
        };

    }
}