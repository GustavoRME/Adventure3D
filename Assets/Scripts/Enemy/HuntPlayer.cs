using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class HuntPlayer : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float sightDistance = 5f;

    [SerializeField]
    [Range(1, 180)]
    private float sightAngle = 30;

    public Vector3 Target { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void HuntingPlayer()
    {
        if (agent.isStopped)
            agent.isStopped = false;

        agent.speed = speed;

        Target = PlayerController.s_Instance.PlayerPosition;
        
        agent.SetDestination(Target);
    }

    public bool IsSight()
    {
        return IsSightDistance() && IsSightAngle();
    }   

    private bool IsSightDistance()
    {
        return Vector3.Distance(PlayerController.s_Instance.PlayerPosition, transform.position) < sightDistance;
    }

    /// <summary>
    /// Only return true if the angle between player and this component is less than half of sight angle
    /// </summary>
    /// <returns></returns>
    private bool IsSightAngle()
    {       
        return Vector3.Angle(GetDirection(), transform.forward) < sightAngle * .5f;
    }

    /// <summary>
    /// Get the direction from player and this component
    /// </summary>
    /// <returns></returns>
    private Vector3 GetDirection()
    {
        return PlayerController.s_Instance.PlayerPosition - transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        //If is playing change colors
        if (Application.isPlaying)
        {
            Gizmos.color = IsSight() ? Color.green : Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }

        //Draw sight distance
        Gizmos.DrawWireSphere(transform.position, sightDistance);
    }
}
