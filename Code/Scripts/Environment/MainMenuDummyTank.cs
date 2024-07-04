using Godot;
using Code.Scripts.Enemies;

namespace Code.Scripts.Environment;

/// <summary>
/// Controls the dummy tank in the main menu
/// </summary>
public partial class MainMenuDummyTank : Node2D
{
    public NavigationController NavigationController;
    public override void _Ready()
    {
        NavigationController = GetNode<NavigationController>("../NavigationController");
    }

    /// <summary>
    /// Rotates the tank towards the mouse position and navigates towards it.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta)
    {
        LookAt(GetGlobalMousePosition());
        NavigationController.NavigateTowards(GetGlobalMousePosition());
    }
}