using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Wander")]
[Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
      "specified velocity.")]

public class WanderBehaviour : BBUnity.Actions.GOAction
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
        agent = agent.GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        targetSpeed = agent.GetComponent<NavMeshAgent>().speed;
        tankMovement = Tank.GetComponent<TankMovement>();

        return base.OnLatentStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && tankMovement.hasShells)
        {
            SetNewDestination(50.0f);
        }

        //Calculate current speed
        Vector3 curMove = Tank.transform.position - previousPosition;
        currentSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = Tank.transform.position;

        //If tanks gets stucked, set another destination
        if (currentSpeed <= targetSpeed * 0.35f)
        {
            SetNewDestination(200.0f);
        }

        return TaskStatus.COMPLETED;

    } // OnUpdate

    public void SetNewDestination(float distance)
    {
        Vector3 current_position = Tank.transform.position; //Get current position
        agent.destination = Wander(current_position, distance, 1);
    }

    public static Vector3 Wander(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

} // class WanderBehaviour
