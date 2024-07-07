namespace Code.Scripts.Enemies;

/// <summary>
/// Dummy behaviour, which only rotates towards the player and does nothing else.
/// </summary>
public partial class DummyBehaviour : Behaviour
{
    public override void Setup()
    {
    }

    /// <summary>
    /// Rotate the gun towards the player.
    /// </summary>
    public override void ExecuteBehaivour()
    {
        Gun.RotateTowards(Player.GlobalPosition);
    }
}
