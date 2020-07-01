using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{   
    private Rigidbody rb;        
    private Vector3 startPosition;
    private int damage;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {        
        startPosition = transform.position;
    }

    private void OnDisable()
    {
        transform.position = startPosition;
    }   

    private void OnCollisionEnter(Collision collision)
    {
        //If collider with someone enemy
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //Apply damage
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }

        //Disable this bullet
        gameObject.SetActive(false);
    }

    public void Fire(int damage, float speed, Vector3 forward)
    {
        //Set the damage will be done if collider with enemy
        this.damage = damage;

        //Set the forward with the current speed 
        Vector3 forwardForce = forward * speed;

        //Change velocity
        rb.velocity = forwardForce;
    }
    
}
