using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle = null;

    private Animation anim;

    private bool isClosed = true;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void OpenBox()
    {
        if(isClosed)
        {
            anim.Play();
            isClosed = false;
        }
    }    

    public void PlayParticle()
    {
        if (!particle.isPlaying)
        {
            particle.Play();

            EllenHealth.Instance.IncreaseHealth();
        }
    }

}
