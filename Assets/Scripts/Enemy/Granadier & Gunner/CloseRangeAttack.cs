using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeAttack : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float punchRange = 4f;

    [SerializeField]
    [Tooltip("Used in the close range attack animation")]
    private Transform leftHand;

    [SerializeField]
    [Tooltip("Used in the melee attack animation")]
    private Transform rightHand;

    [Header("Raycast")]
    [SerializeField]
    private float radius;
    
    [Space]
    [SerializeField]
    [Tooltip("time to execute the Close range attack animation")]
    private float closeRangeCooldown = 10f;
    
    //Take the current time when the close range animation is called. Used to know if the cooldown has passed.
    [SerializeField]private float closeRangeTime;

    //Check if already collide with player. Used to only did one damage for each animation
    private bool isCollided;

    //it's set how true if the event on animation is called
    public bool IsStarted { get; private set; }

    public void PunchAttack()
    {
        if (!isCollided)
        {
            if (HasCollidedPlayer(leftHand))
                HitPlayer();                                    
        }
    }

    public void CloseRange()
    {
        if (!isCollided)
        {
            if (HasCollidedPlayer(rightHand))
                HitPlayer();                
        }

        closeRangeTime = Time.time;
    }

    public void StartAttack()
    {
        IsStarted = true;
    }

    public void EndAttack()
    {
        IsStarted = false;
        isCollided = false;
    }

    /// <summary>
    /// Return true if has the requirements to use the melee attack animation
    /// </summary>
    /// <returns></returns>
    public bool CanMeleeAttack()
    {
        //To use the melee attack, it's only need the player is insde the range
        return IsRange();
    }

    /// <summary>
    /// Return true if has the requirements to use the close range attack animation
    /// </summary>
    /// <returns></returns>
    public bool CanCloseRangeAttack()
    {
        //To use the Close range attack its need the player is inside the range and the isn't in the cooldown
        return IsRange() && !HasCooldown();
    }

    /// <summary>
    /// Check if player is the range of attack
    /// </summary>
    /// <returns></returns>
    private bool IsRange()
    {        
        return Vector3.Distance(transform.position, PlayerController.s_Instance.PlayerPosition) < punchRange;
    }

    /// <summary>
    /// Return true if the CheckSphere collide with player layer
    /// </summary>
    /// <param name="hand">Position of the hand</param>
    /// <returns></returns>
    private bool HasCollidedPlayer(Transform hand)
    {        
        if(Physics.SphereCast(hand.position, radius, hand.forward, out RaycastHit hit))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    /// <summary>
    /// Return true if already in the cooldown to close range attack animation
    /// </summary>
    /// <returns></returns>
    private bool HasCooldown()
    {                
        return Time.time - closeRangeTime < closeRangeCooldown;
    }

    private void HitPlayer()
    {
        EllenHealth.Instance.TakeDamage(damage, transform.position);

        isCollided = true;
    }

    private void OnDrawGizmos()
    {
        if(leftHand)
        {
            //Draw raycast in the left hand
            Gizmos.color = Color.green;
            Gizmos.DrawRay(leftHand.position, leftHand.forward);
        }

        if(rightHand)
        {
            //Draw raycast in the right hand
            Gizmos.color = Color.green;
            Gizmos.DrawRay(rightHand.position, rightHand.forward);
        }

        //Draw range line
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + transform.up, (transform.position + transform.up) + transform.forward * punchRange);
    }
}
