using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    [Header("Shield")]
    [SerializeField]
    private float shieldRange = 5f;

    [SerializeField]
    private float shieldCooldown = 5f;

    [SerializeField]
    private Transform shieldOrigin;

    [Header("Projectil")]
    [SerializeField]
    private GameObject projectilPrefab;

    [SerializeField]
    private Transform projectilOrigin;

    [SerializeField]
    private float projectilRange = 5f;

    [SerializeField]
    private float projectilCooldown = 10f;

    private float shieldActiveTime;
    private float projectilActiveTime;
    
    private void ShieldActive()
    {
        Vector3 dir = PlayerController.s_Instance.PlayerPosition - shieldOrigin.position;

        if(Physics.Raycast(shieldOrigin.position, dir, out RaycastHit hit, shieldRange))
        {
            if(hit.collider.CompareTag("Player"))
            {
                EllenHealth.Instance.TakeDamage(damage, transform.position);
            }
            
        }
        
        shieldActiveTime = Time.time;
    }

    private void ShootProjectil()
    {
        //Shoot the projectil in straight for direction the line range
        Debug.Log("Shoot Projectil");

        projectilActiveTime = Time.time;
    }

    //Called by the animation event
    public void ActivateShield()
    {
        ShieldActive();
    }

    //Called by the animation event
    public void Shoot()
    {
        ShootProjectil();
    }

    public bool CanActivedShield()
    {
        return IsRange(shieldRange) && !HasCooldown(shieldActiveTime, shieldCooldown);
    }

    public bool CanShootProcjetil()
    {
        return IsRange(projectilRange) && !HasCooldown(projectilActiveTime, projectilCooldown);      
    }

    private bool IsRange(float range)
    {
        return Vector3.Distance(PlayerController.s_Instance.PlayerPosition, transform.position) < range;
    }

    private bool HasCooldown(float actionTime, float cooldown)
    {
        return Time.time - actionTime < cooldown;
    }

    private void OnDrawGizmosSelected()
    {
        if(shieldOrigin)
        {
            //Draw shield range
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(shieldOrigin.position, shieldRange);
        }

        if(projectilOrigin)
        {
            //Draw projectil direction
            Gizmos.color = Color.red;
            Gizmos.DrawLine(projectilOrigin.position, projectilOrigin.position + projectilOrigin.forward * projectilRange);
        }
    }
}
