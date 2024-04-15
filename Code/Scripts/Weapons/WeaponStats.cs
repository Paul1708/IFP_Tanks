using Godot;

[GlobalClass]
public partial class WeaponStats : Resource
{
    [Export] public PackedScene bulletScene;
    [Export] public int damage;
    [Export] public float bulletsPerSecond;
    [Export] public float bulletSpeed;
}