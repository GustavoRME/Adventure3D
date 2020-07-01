using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BiteAttack : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private Transform mouth = null;

    [SerializeField]
    private float distanceToAttack = 2f;

    [SerializeField]
    private float biteRadius = 4f;

    public bool IsActived { get; private set; }

    private bool wasHit;

    public void Bite()
    {
        if (!wasHit)
        {
            if (Physics.SphereCast(mouth.position, biteRadius, mouth.forward, out RaycastHit hit, 1f))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    EllenHealth.Instance.TakeDamage(damage, transform.position);
                    wasHit = true;
                }
            }
        }
    }

    public bool CanBite()
    {
        return Vector3.Distance(transform.position, PlayerController.s_Instance.PlayerPosition) < distanceToAttack;
    }
   
    public void AttackBegin()
    {
        IsActived = true;
        wasHit = false;
    }

    public void AttackEnd()
    {
        IsActived = false;
    }

    private void OnDrawGizmos()
    {
        //Distance from attack
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distanceToAttack);

        //Bite Line Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mouth.position, biteRadius);
    }
}
