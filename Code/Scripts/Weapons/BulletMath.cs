namespace Weapons;
using Godot;

public class BulletMath
{
    private BulletMath(){/*Hide default constructor*/}

     /**
     * Check if the given toCheck float is inside the interval [-interval;interval].
     */
    public static bool InSymmetricInterval(float interval, float toCheck)
    {
        return toCheck >= -1.0 * interval && toCheck <= interval;
    }
    
     public static float NormalizeAngle(float angle)
    {
        if (angle > Mathf.Pi)
        {
            return angle - 2 * Mathf.Pi;
        }

        if (angle < -Mathf.Pi)
        {
            return 2 * Mathf.Pi - angle;
        }

        return angle;
    }

    /**
     * Get a vector perpendicular to the shoot-direction to act as a gravitational vector for the grenade trajectory
     * that is oriented to the x-Axis. Note that this vector is normalized.
     */
    public static Vector2 GetSimulatedGravityVector(Vector2 direction)
    {
        //check if the angle is closer to the y or x axis. If closer to x, rotate by +pi/2 angle, if closer to y, rotete by -pi/2
        float angle = direction.Angle();
        float axisSign = InSymmetricInterval(Mathf.Pi / 4, angle) || angle > 3*Mathf.Pi / 4 || angle < -3*Mathf.Pi / 4 ? 1 : -1;
        
        return direction.Rotated(axisSign * Mathf.Pi / 2);
    }

    /**
     * Returns the offset angle in radians of the grenade trajectory to simulate a parabola in 3d on a 2d grid.
     * The angle parameter describes the angle on which the grenade was shot to determine the result offset angle.
     * The arcAngleOffsetDeg parameter describes the returned offset angle in degrees.
     */
    public static float GetArcAngleOffset(float angle, float arcAngleOffsetDeg)
    {
        if (Mathf.Sign(angle) > 0) //below x-Axis
        {
            if (angle < Mathf.Pi / 2.0)
            {
                //if angle larger than 90° then remove offset angle to simulate a grenade flying "upwards"
                return 1 * Mathf.DegToRad(arcAngleOffsetDeg);
            }
            return -1 * Mathf.DegToRad(arcAngleOffsetDeg);
        }
       
        //same for negative angles but signs flipped
        if (angle > -Mathf.Pi / 2.0)
        {
            return -1 * Mathf.DegToRad(arcAngleOffsetDeg);
        }
        
        return +1 * Mathf.DegToRad(arcAngleOffsetDeg);
    }
    
}