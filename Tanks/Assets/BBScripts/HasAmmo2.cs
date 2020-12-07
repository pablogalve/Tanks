using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase

[Condition("MyConditions/HasAmmo2")]
[Help("Checks if the blue tank has ammo")]
public class HasAmmo2 : ConditionBase
{

    public override bool Check()
    {
        GameObject tank = GameObject.FindGameObjectWithTag("bluetank");

        if (tank != null)
        {
            if (tank.GetComponent<TankShooting>().shell_num <= 0)
            {
                //Return to base to recharge
                return true;
            }
        }

        return false;
    }
} // class HasAmmoCondition
