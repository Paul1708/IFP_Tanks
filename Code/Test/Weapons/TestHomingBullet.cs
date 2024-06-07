using Code.Scripts.Weapons;
using GdUnit4;
using Godot;

namespace Code.Test.Weapons;
[TestSuite]
public class TestHomingBullet
{

	[TestCase(1,1, 4,4, 2,-4)]
	[TestCase(0,-7, 14,5.54, 0.2,-9.4)]
	[TestCase(3.7,9.4, -15.5,3.33, -20.7,-30.8)]
	[TestCase(-1,-1, -4,-4, -2,+4)]
	public void TestHoming(float bulletPosX, float bulletPosY, float targetPosX, float targetPosY, float targetMoveX,
		float targetMoveY)
	{
		
		//init test with parametrized target and bullet positions.
		HomingBullet bullet = new HomingBullet();
		Node2D target = new Node2D();
		target.GlobalPosition = new Vector2(targetPosX, targetPosY);

		bullet.TargetNode = target;
		bullet.HomingTicks = 0;
		bullet.GlobalPosition = new Vector2(bulletPosX, bulletPosY);
		
		//init target ray for bullet. Invariant: The bullet uses the direct line from it's position to the target
		//PreCond: Bullets target direction is not set
		Assertions.AssertObject(bullet.TargetDirection).IsEqual(System.Numerics.Vector2.Zero);
		bullet.MoveTest();
		
		//PostCond see above (invariant holds)
		Assertions.AssertObject(bullet.TargetDirection).IsEqual(new Vector2(target.GlobalPosition.X, target.GlobalPosition.Y) - bullet.GlobalPosition);
		
		
		//move the target to test the homing feature of the bullet
		target.GlobalPosition += new Vector2(targetMoveX, targetMoveY);
		
		//Invariant: The new direction points closer to the target than the old one
		//move bullet and check if the invariant holds
		Vector2 oldTargetDirection = bullet.TargetDirection;
		bullet.MoveTest();
		
		//PostCond: Invariant holds
		Vector2 newDirection = bullet.TargetDirection;
		
		float oldLength = (target.GlobalPosition - oldTargetDirection).Length();
		float newLength = (target.GlobalPosition - newDirection).Length();
		Assertions.AssertFloat(newLength).IsLessEqual(oldLength);
	}
	
}
