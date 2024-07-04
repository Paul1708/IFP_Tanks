using Godot;

namespace Code.Scripts.Weapons;

/// <summary>
/// Data class which holds the stats of a weapon
/// </summary>
[GlobalClass]
public partial class WeaponStats : Resource
{
    [Export] public PackedScene BulletScene;
    [Export] public float Damage;
    [Export] public float BulletsPerSecond;
    [Export] public float BulletSpeed;
}