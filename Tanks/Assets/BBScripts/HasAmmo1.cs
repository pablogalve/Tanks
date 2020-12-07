using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase

[Condition("MyConditions/HasAmmo1")]
[Help("Checks if the red tank has ammo")]
public class HasAmmo1 : ConditionBase
{

    public override bool Check()
    {
        GameObject tank = GameObject.FindGameObjectWithTag("redtank");
        
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
