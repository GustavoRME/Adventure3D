using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GruntAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip gruntClip = null;
    private AudioSource source;

    public void Grunt()
    {
        if (source == null)
            source = GetComponent<AudioSource>();

        if (gruntClip != null)
            source.PlayOneShot(gruntClip);
        else
            Debug.Log("Grunt withou clip. GruntAudiou script");
        
    }
}
