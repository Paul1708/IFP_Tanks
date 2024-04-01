using Godot;
using Weapons;

namespace Enemies;

public abstract partial class Behaviour : Node2D
{
    protected GunController Gun { get; set; }
    protected Node2D Player { get; set; }
    protected NavigationController Navigation { get; set; }

    public override void _Ready()
    {
        // Get instances
        Gun = GetParent().GetNode<GunController>("Gun");
        Player = GetTree().GetNodesInGroup("Player")[0] as Node2D;
        Navigation = GetParent().GetNode<NavigationController>("NavigationAgent2D");
        Setup();
    }

    abstract public void ExecuteBehaivour();
    abstract public void Setup();

    public override void _Process(double delta)
    {
        ExecuteBehaivour();
    }
}
