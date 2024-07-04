namespace Code.Scripts.Enemies;

/// <summary>
/// Dummy behaviour, which only rotates towards the player and does nothing else.
/// </summary>
public partial class DummyBehaviour : Behaviour
{
    public override void Setup()
    {
    }
    
    public override void ExecuteBehaivour()
    {
        Gun.RotateTowards(Player.GlobalPosition);
    }
}
