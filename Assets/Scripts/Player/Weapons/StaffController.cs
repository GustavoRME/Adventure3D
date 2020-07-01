using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class StaffController : MonoBehaviour, IWeapon
{
    [SerializeField]
    private int damage = 1;

    [Header("Transform Attachs")]    
    [SerializeField]
    private Transform handAttach = null;

    [SerializeField]
    private Transform backAttach = null;

    [Header("Raycast sphere")]
    
    [SerializeField]
    [Tooltip("Center of the collider")]
    private Transform raycastCenter = null;

    [SerializeField]
    [Tooltip("Size of the raycast")]
    private float areaDamage = 1.0f;

    [SerializeField]
    [Tooltip("Layer to enemy")]
    private LayerMask collisionLayer = 0;

    public bool isHandle { get; set ; }
    
    public int staffCombo;

    public void Attack()
    {
        if(isHandle)
        {
            CreateCollider();
        }
    }

    //Take the weapon
    public void PullTheGun()
    {
        if(!isHandle)
        {
            transform.SetParent(handAttach);

            transform.position = handAttach.position;
            transform.rotation = handAttach.rotation;

            transform.localScale = Vector3.one;

            isHandle = true;
        }
    }

    //Retain the weapon
    public void PutAway()
    {
        //Only put way if the weapon is on the handl
        if(isHandle)
        {
            transform.SetParent(backAttach);

            transform.position = backAttach.position;
            transform.rotation = backAttach.rotation;

            transform.localScale = Vector3.one;

            isHandle = false;
        }
    }   

    private void CreateCollider()
    {
        Collider[] colls = Physics.OverlapSphere(raycastCenter.position, areaDamage, collisionLayer);

        if(colls != null)
        {            
            foreach (Collider coll in colls)
            {
                if(coll.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(damage, staffCombo);
                }
                
                if(coll.TryGetComponent(out HealthBox healthBox))
                {
                    healthBox.OpenBox();
                }

                if(coll.TryGetComponent(out DesctructableBox desctructableBox))
                {
                    Debug.Log("Desctruct box");
                    desctructableBox.DestructBox();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawSphere(raycastCenter.position, areaDamage);
    }
}
