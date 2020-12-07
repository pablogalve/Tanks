using UnityEngine;
using UnityEngine.AI;
using System.Collections;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Patrolling")]
[Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
      "specified velocity.")]

public class PatrollingBehaviour : BBUnity.Actions.GOAction
{
    [InParam("Navmesh")]
    public NavMeshAgent agent;

    [InParam("Tank")]
    public GameObject Tank;

    private Transform[] points = new Transform[7];
    private GameObject[] waypoints = new GameObject[7];
    private int destPoint = 0;

    TankMovement tankMovement;

    public override TaskStatus OnLatentStart()
    {
        agent = agent.GetComponent<NavMeshAgent>();
        tankMovement = Tank.GetComponent<TankMovement>();
        agent.autoBraking = false;
        
        for (int i = 0; i < points.Length; i++)
        {
            waypoints[i] = GameObject.FindGameObjectsWithTag("waypoint")[i];
            points[i] = waypoints[i].transform;
        }
        if (tankMovement.hasShells == true)
            GotoNextPoint();
        else
        {
            tankMovement.GoToBase();
        }

        return base.OnLatentStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && tankMovement.hasShells)
            GotoNextPoint();

        Debug.Log(tankMovement.hasShells);
        Debug.Log(agent.remainingDistance);
        return TaskStatus.COMPLETED;

    } // OnUpdate

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        Debug.Log(points[destPoint].position);
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;

    }

} // class PatrollingBehaviour
