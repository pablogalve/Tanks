using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Patrol : MonoBehaviour {

    public Transform[] points;
    public GameObject[] waypoints;
    private int destPoint = 0;
    private NavMeshAgent agent;

    TankMovement tankMovement;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        tankMovement = GetComponent<TankMovement>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        for (int i = 0; i < points.Length; i++)
        {
            waypoints[i] = GameObject.FindGameObjectsWithTag("waypoint")[i];
            points[i] = waypoints[i].transform;
        }
        if(tankMovement.hasShells == true)
            GotoNextPoint();
        else{
            tankMovement.GoToBase();
        }   
    }

    void Update () {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && tankMovement.hasShells)
            GotoNextPoint();
    }

    void GotoNextPoint() {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }
}