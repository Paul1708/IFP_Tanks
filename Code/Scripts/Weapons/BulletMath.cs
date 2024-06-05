
using Godot;

namespace Code.Scripts.Weapons;

public class BulletMath
{
    private BulletMath(){/*Hide default constructor*/}
    
     /**
      * Convert an angle from the interval J = [0,2*Pi] to an angle in I = [-Pi,Pi] where 0 in J is mapped to 0 in I.
      * This is needed since the godot angle function uses angles in I.
      */
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
     * Ensure that the angle does not exceed the interval [-maxAngleRadians, maxAngleRadians]. If it is not inside
     * this interval, the closed interval border will be returned.
     */
    public static float RestrictHomingAngle(float angle, float maxAngleRadians)
    {
        if (Mathf.Sign(angle) < 0)
        {
            return Mathf.Max(angle, -maxAngleRadians);
        }
        else
        {
            return Mathf.Min(angle, maxAngleRadians);
        }
    }
    
}