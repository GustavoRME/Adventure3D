using System.Collections;
using System.Linq;
using UnityEngine;

public class EllenHealthUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time to wait until start a new animation increase or decrease")]
    private float waitTime = 0.2f;
    
    [Space]

    [SerializeField]
    [Tooltip("Animator that control the animation from health icon")]
    private Animator[] healthAnimators = null;
    
    [SerializeField]
    private int health;

    private void Start()
    {
        health = EllenHealth.Instance.Health;       
        EllenHealth.Instance.OnDecreaseHealth += () => StartCoroutine(DecreaseHealth());
        EllenHealth.Instance.OnIncreaseHealth += () => StartCoroutine(IncreaseHealth());
    }
    
    private void OnDisable()
    {
        EllenHealth.Instance.OnDecreaseHealth -= () => StartCoroutine(DecreaseHealth());
        EllenHealth.Instance.OnIncreaseHealth -= () => StartCoroutine(IncreaseHealth());
    }

    private IEnumerator DecreaseHealth()
    {
        //Only decrease if have health
        if(health > 0)
        {            
            int currentHealth = EllenHealth.Instance.Health;

            while (health > currentHealth)
            {
                //Decrease first to already use as index. This way will not use one index equals as length.
                health--;   

                SetTrigger("Losed", healthAnimators[health]);

                yield return new WaitForSeconds(waitTime);                
            }
        }
    }

    private IEnumerator IncreaseHealth()
    {
        if(health < healthAnimators.Length)
        {
            int currentHealth = EllenHealth.Instance.Health;

            while (health < currentHealth)
            {                
                SetTrigger("Gained", healthAnimators[health]);

                yield return new WaitForSeconds(waitTime);

                //Increase later to can use 0 how index. 
                health++;        
            }
        }
    }    

    private void SetTrigger(string trigger, Animator anim)
    {
        anim.SetTrigger(trigger);
    }   
}
