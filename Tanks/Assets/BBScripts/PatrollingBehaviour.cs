using UnityEngine;
using UnityEngine.AI;
using System.Collections;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Patrolling")]
[Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
      "specified velocity.")]

public class Patrolling : BBUnity.Actions.GOAction
{
    [InParam("Navmesh")]
    public NavMeshAgent agent;

    [InParam("Tank")]
    public GameObject Tank;

    [InParam("Ghost")]
    public GameObject[] Ghost;

    private Transform[] points;
    private GameObject[] waypoints;
    private int destPoint = 0;
    private bool SetPoints = true;
    TankMovement tankMovement;


    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        agent = agent.GetComponent<NavMeshAgent>();
        tankMovement = Tank.GetComponent<TankMovement>();
        Ghost = GameObject.FindGameObjectsWithTag("ghost");
        agent.autoBraking = false;

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (tankMovement.hasShells)
            GotoNextPoint();

        return TaskStatus.COMPLETED;

    } // OnUpdate

    void GotoNextPoint()
    {
        agent.destination = Ghost[0].transform.position;
    }

} // class PatrollingBehaviour
