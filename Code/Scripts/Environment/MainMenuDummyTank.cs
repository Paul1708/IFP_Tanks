using Godot;

public partial class MainMenuDummyTank : Node2D
{
    public NavigationController NavigationController;
    public override void _Ready()
    {
        NavigationController = GetNode<NavigationController>("../NavigationController");
    }
    public override void _Process(double delta)
    {
        LookAt(GetGlobalMousePosition());
        NavigationController.NavigateTowards(GetGlobalMousePosition());
    }
}