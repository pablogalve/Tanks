using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 previousPosition;
    public float currentSpeed;
    public float targetSpeed;

    TankMovement tankMovement;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        targetSpeed = GetComponent<NavMeshAgent>().speed;
        tankMovement = GetComponent<TankMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && tankMovement.hasShells)
        {
            SetNewDestination(50.0f);
        }

        //Calculate current speed
        Vector3 curMove = transform.position - previousPosition;
        currentSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        //If tanks gets stucked, set another destination
        if(currentSpeed <= targetSpeed * 0.35f)
        {
            SetNewDestination(200.0f);
        }
    }

    public void SetNewDestination(float distance)
    {
        Vector3 current_position = transform.position; //Get current position
        agent.destination = Wander(current_position, distance, 1);
    }

    public static Vector3 Wander (Vector3 origin, float distance, int layermask) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

            randomDirection += origin;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
           
            return navHit.position;
        }
}
