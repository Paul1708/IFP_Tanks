using System.Collections.Generic;
using System.Security.Cryptography;
using Components;
using GdUnit4;
using Godot;
using Managers;
using Movement;

[TestSuite]
public class TestNavigationController
{
    CharacterBody2D cb;
    NavigationController nc;
    [BeforeTest]
    public void Setup()
    {
        cb = new CharacterBody2D();
        nc = new NavigationController();

        cb.Position = new Vector2(0, 0);
        cb.Velocity = new Vector2(0, 1);
        cb.Rotation = 0;
    }

    /*
    I really have no clue, why this test always passes.
    Can't even get to access the properties the right way.
    
    [TestCase]
    public void MoveTowardsVectorCorrectlyLerpsAngle()
    {
        GD.Print("RotationStart: " + cb.Rotation);
        GD.Print("VelocityStart: " + cb.Velocity);
        GD.Print("PositionStart: " + cb.Position);

        nc.RotationSpeed = 0.5f;
        nc.MovementSpeed = 2f;
        nc.characterBody = cb;

        nc.MoveTowardsVector(new Vector2(1, 0));
        cb.MoveAndSlide();
        cb._PhysicsProcess(0.1f);

        GD.Print("RotationEnd: " + cb.Rotation);
        GD.Print("VelocityEnd: " + cb.Velocity);
        GD.Print("PositionEnd: " + cb.Position);

        Assertions.AssertFloat(cb.Rotation).Equals(0.5f);
        Assertions.AssertVec2(cb.Velocity).Equals(new Vector2(1, 0));
    }
    */

    [AfterTest]
    public void TearDown()
    {
        cb.QueueFree();
        nc.QueueFree();
    }


}