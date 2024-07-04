using System;
using Code.Scripts.Environment;
using Godot;

namespace Code.Scripts.Components;

/// <summary>
/// Controls the spawning of the DamageNumber scene
/// </summary>
public partial class DamageNumberController : Node2D
{
    public PackedScene DamageNumberScene { get; set; }

    public override void _Ready()
    {
        // Conenect to the OnTakeDamage event of the HealthComponent and load the DamageNumber scene
        GetParent<HealthComponent>().OnTakeDamage += SpawnDamageNumber;
        DamageNumberScene = GD.Load<PackedScene>("res://Scenes/Enviroment/DamageNumber.tscn");
    }
    public override void _ExitTree()
    {
        GetParent<HealthComponent>().OnTakeDamage -= SpawnDamageNumber;
    }

    /// <summary>
    /// Spawns a new DamageNumber scene with the damage value
    /// </summary>
    /// <param name="damage">The damage value</param>
    public void SpawnDamageNumber(float damage)
    {
        // Instantiate a new DamageNumber scene and set the text to the damage value
        DamageNumber damageNumber = DamageNumberScene.Instantiate<DamageNumber>();
        damageNumber.Text = Math.Round(damage, 1).ToString();

        // Set the position to a little bit above the parent's position and add it to the Tree
        damageNumber.GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y - 50);
        GetTree().GetFirstNodeInGroup("LevelManager").AddChild(damageNumber);

    }
}