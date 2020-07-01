using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StepAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip stepClip = null;

    private AudioSource audioSource;

   
    public void PlayStep()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (stepClip)
            audioSource.PlayOneShot(stepClip);
        else
            Debug.Log("Play step audio");        
    }
}
