using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolController : MonoBehaviour, IWeapon
{
    //Tag to get the bullet from the object pooler
    private const string BULLET_TAG = "Bullet";

    [Header("Weapon properties")]

    [SerializeField]
    [Tooltip("Damage to apply for the enemy")]
    private int damage = 2;

    [SerializeField]
    [Tooltip("Amount of current bullets have")]
    private int ammoAmount = 10;

    [SerializeField]
    [Tooltip("Speed of the bullet")]
    private float bulletSpeed = 30f;

    [Header("Attaches")]
    
    [SerializeField]
    [Tooltip("Place to attach when the gun is stored")]
    private Transform holster = null;   

    [SerializeField]
    [Tooltip("Place to attach when the gun is take")]
    private Transform hand = null;        

    public bool isHandle { get; set ; }

    public void Attack()
    {
        //Only shot if the piston is on the hands
        if (isHandle)
        {
            //Only fire if have ammo
            if (ammoAmount > 0)
            {
                //Get from the pooling one bullet
                GameObject go = ObjectPoolerController.s_Instance.GetPoolItem(BULLET_TAG);

                if (go == null)
                    return;

                //Get the bullet component
                Bullet bullet = go.GetComponent<Bullet>();

                //Active it
                bullet.gameObject.SetActive(true);

                //Remove the parent
                bullet.transform.SetParent(null);

                //Fire -> the face of the pistol is the red axis, Y 
                bullet.Fire(damage, bulletSpeed, transform.right);

                //Decrease the amount of bullets
                DecreaseBullets();

                Debug.Log("Pistol shot");
            }
        }
    }

    public void PutAway()
    {
        //If already isn't in the hands, don't do anything
        if (!isHandle)
            return;

        //Call Animation       

        //Put how parent of the holster
        transform.SetParent(holster);

        //Fix in place
        transform.position = holster.position;
        transform.eulerAngles = holster.eulerAngles;

        //Control the scale. When is switched, for some reason the scale is raising
        transform.localScale = Vector3.one;
        
        isHandle = false;        
    }

    public void PullTheGun()
    {
        //If already in the hand, don't do anything
        if (isHandle)
            return;

        //Call Animation

        //Put how parent of the hand
        transform.SetParent(hand);

        //Fix in place
        transform.position = hand.position;
        transform.eulerAngles = hand.eulerAngles;

        //Control the scale. When is switched, for some reason the scale is raising
        transform.localScale = Vector3.one;

        isHandle = true;        
    }

    public void IncreaseBullets()
    {        
        ammoAmount++;
    }

    public void DecreaseBullets()
    {        
        //Decrease the current amount the bullets is greater than zero, stay decrease one, else stay in zero
        ammoAmount = ammoAmount > 0 ? ammoAmount - 1 : 0;
    }  

    public bool HaveBullets()
    {
        return ammoAmount > 0;
    }
}
