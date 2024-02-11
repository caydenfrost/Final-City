using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public CharacterMove DragCharacter;
    public bool isWandering;
    public float wanderRadius;
    public NavMeshAgent agent;
    public float speed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null )
        {
            Debug.LogError("NavMeshAgent not found");
            return;
        }
    }

    void Update()
    {
        if (DetectFloor())
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            Vector3 finalPosition = hit.position;

            
            agent.SetDestination(finalPosition);
        }
        agent.speed = speed * Time.deltaTime;
    }
    bool DetectFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                return true;
            }
        }
        return false;
    }
}

