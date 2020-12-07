using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Reload")]
[Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
      "specified velocity.")]

public class ReloadAmmo : BBUnity.Actions.GOAction
{
    [InParam("Navmesh")]
    public NavMeshAgent agent;

    [InParam("Tank")]
    public GameObject Tank;

    private Vector3 previousPosition;
    public float currentSpeed;
    public float targetSpeed;

    TankMovement tankMovement;

    public override TaskStatus OnLatentStart()
    {
       

        return base.OnLatentStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        

        return TaskStatus.COMPLETED;

    } // OnUpdate


}
