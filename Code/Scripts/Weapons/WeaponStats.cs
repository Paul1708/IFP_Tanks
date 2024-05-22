using Godot;

[GlobalClass]
public partial class WeaponStats : Resource
{
    [Export] public PackedScene bulletScene;
    [Export] public float damage;
    [Export] public float bulletsPerSecond;
    [Export] public float bulletSpeed;
}