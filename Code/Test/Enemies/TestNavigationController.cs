
using Code.Scripts.Enemies;
using GdUnit4;
using Godot;

namespace Code.Test.Enemies;

[TestSuite]
public class TestNavigationController
{
    CharacterBody2D _cb;
    NavigationController _nc;
    [BeforeTest]
    public void Setup()
    {
        _cb = new CharacterBody2D();
        _nc = new NavigationController();

        _cb.Position = new Vector2(0, 0);
        _cb.Velocity = new Vector2(0, 1);
        _cb.Rotation = 0;
    }
    

    [AfterTest]
    public void TearDown()
    {
        _cb.QueueFree();
        _nc.QueueFree();
    }


}