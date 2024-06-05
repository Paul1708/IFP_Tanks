namespace Code.Scripts.Enemies;

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
