using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    [Tooltip("Range the enemy will be walking while is patrolling")]
    private float patrolRange = 10f;

    [SerializeField]
    [Tooltip("Time to change for the next destination into the patrol range")]
    private float changeDestinationTime = 30f;
    private float elapsedTime;          
    
    public Vector3 Target { get; private set; }
    private Vector3 PatrolCenter { get; set; }       
                
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        PatrolCenter = transform.position;

        elapsedTime = changeDestinationTime;
    }

    public void Patrolling()
    {
        agent.speed = speed;

        if (agent.isStopped)
            agent.isStopped = false;

        if(elapsedTime > changeDestinationTime)
        {
            Target = GetDestination();

            agent.SetDestination(Target);

            elapsedTime = 0.0f;            
        }

        elapsedTime += Time.fixedDeltaTime;
    }

    /// <summary>
    /// Return one vector3 point inside the sphere
    /// </summary>
    /// <returns></returns>
    private Vector3 GetDestination()
    {
        Vector2 circle = Random.insideUnitCircle * patrolRange;
        
        return new Vector3(circle.x, 0.0f, circle.y) + PatrolCenter;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Vector3 center = Application.isPlaying ? PatrolCenter : transform.position;

        Gizmos.DrawWireSphere(center, patrolRange);
    }
}
